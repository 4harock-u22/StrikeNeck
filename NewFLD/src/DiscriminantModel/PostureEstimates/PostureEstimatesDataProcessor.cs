using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;

/* プロジェクト 'ForwardLeanDetection (net7.0)' からのマージされていない変更
前:
using Microsoft.ML;
後:
using Microsoft.ML;
using NewFLD;
using NewFLD.PostureEstimates;
using ForwardLeanDetection.src.DiscriminantModel.PostureEstimates;
*/
using Microsoft.ML;

namespace DiscriminantModel.PostureEstimates
{

    //前処理
    internal class PostureEstimatesDataPretreatment
    {
        internal List<NamedOnnxValue> convertImageToInputList(Image<Rgb24> sourceImage)
        {
            var resizedImage = resizeImage(sourceImage);
            var tensor = makeTensorFlomImage(resizedImage);
            var inputList = makeInputList(tensor);

            return inputList;
        }
        private Image<Rgb24> resizeImage(Image<Rgb24> imageBeforeResizing)
        {
            const int height = PostureEstimatesResource.predictedImagesHeight;
            const int width = PostureEstimatesResource.predictedImagesWidth;
            var imageAfterResizing = imageBeforeResizing.Clone(x => x.Resize(width, height));

            return imageAfterResizing;
        }

        private Tensor<float> makeTensorFlomImage(Image<Rgb24> sourceImage)
        {
            var tensor = new DenseTensor<float>(new[] { 1, 257, 257, 3 });
            var mean = PostureEstimatesResource.mean;
            var stddev = PostureEstimatesResource.stddev;


            sourceImage.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgb24> pixelSpan = accessor.GetRowSpan(y);
                    for (int x = 0; x < accessor.Width; x++)
                    {
                        tensor[0, y, x, 0] = (pixelSpan[x].R / 255f - mean[0]) / stddev[0];
                        tensor[0, y, x, 1] = (pixelSpan[x].G / 255f - mean[1]) / stddev[1];
                        tensor[0, y, x, 2] = (pixelSpan[x].B / 255f - mean[2]) / stddev[2];
                    }
                }
            });
            return tensor;
        }

        private List<NamedOnnxValue> makeInputList(Tensor<float> tensor)
        {
            var inputList = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("sub_2", tensor)
            };

            return inputList;
        }



        internal InferenceSession buildSession()
        {
            var resource = new PostureEstimatesResource();
            
            var posenetStreamReader = resource.posenetStreamReader;
            var memoryStream = new MemoryStream();
            byte[] modelByetes;

            posenetStreamReader.BaseStream.CopyTo(memoryStream);
            modelByetes = memoryStream.ToArray();


            var model = new InferenceSession(modelByetes);

            return model;
        }

    }

    //後処理
    internal class PostureEstimatesDataPostProcessor
    {

        internal Dictionary<KeyPointType, TwoDPoint> calcKeyPointsFromResults(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results)
        {
            var resultsList = convertResultsToTensorList(results);
            var heatmaps = resultsList[OUTPUT_TYPE.HEATMAP];
            var offsets = resultsList[OUTPUT_TYPE.OFFSET];

            var maxHeatmaps = findMaxHeatmaps(heatmaps);
            var offsetVectors = calcOffsetVectors(offsets, maxHeatmaps);
            var keyPoints = calcKeyPoints(offsetVectors, maxHeatmaps);

            return keyPoints;
        }

        private Dictionary<KeyPointType, TwoDPoint> calcKeyPoints(Dictionary<KeyPointType, TwoDPoint> offsetsVectors, float[][] maxHeatmaps)
        {

            var keyPoints = new Dictionary<KeyPointType, TwoDPoint>();
            foreach (var keyPointType in Enum.GetValues<KeyPointType>())
            {
                var offsetVector = offsetsVectors[keyPointType];
                var keyPoint = calcKeyPoint(offsetVector, maxHeatmaps[(int)keyPointType]);

                keyPoints.Add(keyPointType, keyPoint);
            }

            return keyPoints;
        }

        private TwoDPoint calcKeyPoint(TwoDPoint offsetVector, float[] maxHeatmap)
        {
            var keyPointY = maxHeatmap[0] * PostureEstimatesResource.intervalOfHeatmap + offsetVector.x;
            var keyPointX = maxHeatmap[1] * PostureEstimatesResource.intervalOfHeatmap + offsetVector.y;

            var keyPoint = new TwoDPoint(keyPointX, keyPointY);

            return keyPoint;
        }

        private Dictionary<KeyPointType, TwoDPoint> calcOffsetVectors(Tensor<float> offset, float[][] maxHeatmaps)
        {
            var offsetVectors = new Dictionary<KeyPointType, TwoDPoint>();
            var numberOfKeyPoints = PostureEstimatesResource.numberOfKeyPoints;

            foreach (var keyPointType in Enum.GetValues<KeyPointType>())
            {
                var integerOfKey = (int)keyPointType;
                var pointX = offset[0, (int)maxHeatmaps[integerOfKey][0], (int)maxHeatmaps[integerOfKey][1], integerOfKey];
                var pointY = offset[0, (int)maxHeatmaps[integerOfKey][0], (int)maxHeatmaps[integerOfKey][1], integerOfKey + numberOfKeyPoints];

                var offsetVector = new TwoDPoint(pointX, pointY);
                offsetVectors.Add(keyPointType, offsetVector);
            }

            return offsetVectors;
        }

        private float[][] findMaxHeatmaps(Tensor<float> heatmaps)
        {
            var height = PostureEstimatesResource.heatmapsHeight;
            var width = PostureEstimatesResource.heatmapsWidth;
            var numberOfKeyPoints = PostureEstimatesResource.numberOfKeyPoints;

            var heatmapSoresMax = new float[numberOfKeyPoints, 3];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    foreach (var keyId in Enum.GetValues<KeyPointType>())
                    {
                        var keyIdInt = (int)keyId;

                        var heatmapScore = sigmoid(heatmaps[0, y, x, keyIdInt]);

                        if (heatmapScore < heatmapSoresMax[keyIdInt, 2]) continue;

                        heatmapSoresMax[keyIdInt, 0] = y;
                        heatmapSoresMax[keyIdInt, 1] = x;
                        heatmapSoresMax[keyIdInt, 2] = (float)heatmapScore;
                    }
                }
            }
            var arrangedHeatmapsMax = new float[numberOfKeyPoints][];

            for (int i = 0; i < numberOfKeyPoints; i++)
            {
                arrangedHeatmapsMax[i] = new float[3];
                arrangedHeatmapsMax[i][0] = heatmapSoresMax[i, 0];
                arrangedHeatmapsMax[i][1] = heatmapSoresMax[i, 1];
                arrangedHeatmapsMax[i][2] = heatmapSoresMax[i, 2];
            }

            return arrangedHeatmapsMax;
        }

        private double sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        private Dictionary<OUTPUT_TYPE, Tensor<float>> convertResultsToTensorList(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results)
        {
            var heatmaps = results[0].AsTensor<float>();
            var offsets = results[1].AsTensor<float>();
            var displacementFwd = results[2].AsTensor<float>();
            var displacementBwd = results[3].AsTensor<float>();

            var resultsList = new Dictionary<OUTPUT_TYPE, Tensor<float>>
            {
                { OUTPUT_TYPE.HEATMAP, heatmaps },
                { OUTPUT_TYPE.OFFSET, offsets },
                { OUTPUT_TYPE.DISPLACEMENT_FWD, displacementFwd },
                { OUTPUT_TYPE.DISPLACEMENT_BWD, displacementBwd }
            };

            return resultsList;
        }
    }

    enum OUTPUT_TYPE
    {
        HEATMAP,
        OFFSET,
        DISPLACEMENT_FWD,
        DISPLACEMENT_BWD
    }
}
