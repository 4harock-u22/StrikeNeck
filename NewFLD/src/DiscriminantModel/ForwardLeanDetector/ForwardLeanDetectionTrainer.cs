using DiscriminantModel.PostureEstimates;
using Image = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24>;
using SixLabors.ImageSharp.PixelFormats;
using NewFLD;
using Microsoft.ML;
using System.Reflection;
using NewFLD.src.DiscriminantModel.ForwardLeanDetector;

namespace DiscriminantModel.ForwardLeanDetector
{
    class ForwardLeanDetectionTrainer
    {

        internal async Task Retrain(DirectoryInfo currectPosture, DirectoryInfo forwardLeanPosture)
        {
            var trainData = await this.BuildTrainData(currectPosture, forwardLeanPosture);
            
            var modelFileInfo = await new ModelFileAccessor().GetModelFileInfo();


            var context = new MLContext();
            var inputData = context.Data.LoadFromEnumerable<MLModel1.ModelInput>(trainData);
            var newModel = MLModel1.RetrainPipeline(context, inputData);
            context.Model.Save(newModel, inputData.Schema, modelFileInfo.FullName);
        }

        private async Task<IEnumerable<MLModel1.ModelInput>> BuildTrainData(DirectoryInfo currectPosture, DirectoryInfo forwardLeanPosture)
        {
            var currentInputs = await this.posturesEstimate(currectPosture, false);

            var forwardLeanInputs = await this.posturesEstimate(forwardLeanPosture, true);

            var modelInputsList = new List<MLModel1.ModelInput>();
            modelInputsList.AddRange(currentInputs);
            modelInputsList.AddRange(forwardLeanInputs);

            return modelInputsList;
        }

         private async Task<List<MLModel1.ModelInput>> posturesEstimate(DirectoryInfo posture, bool result)
         {
            var inputs = new List<MLModel1.ModelInput>();
            foreach (var sample in posture.GetFiles())
            {
                var input = await this.postureEstimate(sample, result);
                inputs.Add(input);
            }

            return inputs;
        }

        private async Task<MLModel1.ModelInput> postureEstimate(FileInfo sample, bool res)
        {
            var postureEstimator = new PostureEstimatesAPI();
            var sampleImage = Image.Load<Rgb24>(sample.FullName);

            var result = await postureEstimator.Predict(sampleImage);

            float N_LEye_X = result[KeyPointType.Nose].x - result[KeyPointType.LeftEye].x;
            float N_LEye_Y = result[KeyPointType.Nose].y - result[KeyPointType.LeftEye].y;
            float REye_N_X = result[KeyPointType.RightEye].x - result[KeyPointType.Nose].x;
            float REye_N_Y = result[KeyPointType.RightEye].y - result[KeyPointType.Nose].y;
            float LEye_LEar_X = result[KeyPointType.LeftEye].x - result[KeyPointType.LeftEar].x;
            float LEye_LEar_Y = result[KeyPointType.LeftEye].y - result[KeyPointType.LeftEar].y;
            float REar_REye_X = result[KeyPointType.RightEar].x - result[KeyPointType.RightEye].x;
            float REar_REye_Y = result[KeyPointType.RightEar].y - result[KeyPointType.RightEye].y;
            float N_LEar_X = result[KeyPointType.Nose].x - result[KeyPointType.LeftEar].x;
            float N_LEar_Y = result[KeyPointType.Nose].y - result[KeyPointType.LeftEar].y;
            bool fld = res;

            var input = new MLModel1.ModelInput()
            {
                N_LEar_X = N_LEar_X,
                N_LEar_Y = N_LEar_Y,
                REar_REye_X = REar_REye_X,
                REar_REye_Y = REar_REye_Y,
                LEye_LEar_X = LEye_LEar_X,
                LEye_LEar_Y = LEye_LEar_Y,
                REye_N_X = REye_N_X,
                REye_N_Y = REye_N_Y,
                N_LEye_X = N_LEye_X,
                N__Eye_Y = N_LEye_Y,
                FLD = fld.ToString()
            };

            return input;
        }
    }
}
