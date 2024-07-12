using DiscriminantModel.PostureEstimates;
using Image = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24>;
using SixLabors.ImageSharp.PixelFormats;
using NewFLD;
using Microsoft.ML;

namespace DiscriminantModel.ForwardLeanDetector
{

    internal class ModelInput
    {
        public float N_LEar_X { get; set; }

        public float N_LEar_Y { get; set; }

        public float REar_REye_X { get; set; }

        public float REar_REye_Y { get; set; }

        public float LEye_LEar_X { get; set; }

        public float LEye_LEar_Y { get; set; }

        public float REye_N_X { get; set; }

        public float REye_N_Y { get; set; }

        public float N_LEye_X { get; set; }

        public float N__Eye_Y { get; set; }

        public string FLD { get; set; }

    }
    class ForwardLeanDetectionTrainer
    {

        internal void Retrain(DirectoryInfo currectPosture, DirectoryInfo forwardLeanPosture)
        {
            var trainData = this.BuildTrainData(currectPosture, forwardLeanPosture);
            var modelPath = MLModel1.MLNetModelPath;
            var context = new MLContext();
            var inputData = context.Data.LoadFromEnumerable<ModelInput>(trainData);
            var newModel = MLModel1.RetrainPipeline(context, inputData);
            context.Model.Save(newModel, inputData.Schema, modelPath);
        }

        IEnumerable<ModelInput> BuildTrainData(DirectoryInfo currectPosture, DirectoryInfo forwardLeanPosture)
        {
            var currentInputs = this.posturesEstimate(currectPosture, false);

            var forwardLeanInputs = this.posturesEstimate(forwardLeanPosture, true);

            var modelInputsList = new List<ModelInput>();
            modelInputsList.AddRange(currentInputs);
            modelInputsList.AddRange(forwardLeanInputs);

            return modelInputsList;
        }

         List<ModelInput> posturesEstimate(DirectoryInfo posture, bool result)
         {
            var inputs = new List<ModelInput>();
            foreach (var sample in posture.GetFiles())
            {
                var input = this.postureEstimate(sample, result);
                inputs.Add(input);
            }

            return inputs;
        }

        ModelInput postureEstimate(FileInfo sample, bool res)
        {
            var postureEstimator = new PostureEstimatesAPI();
            var sampleImage = Image.Load<Rgb24>(sample.FullName);

            var result = postureEstimator.predict(sampleImage);

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

            var input = new ModelInput()
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
