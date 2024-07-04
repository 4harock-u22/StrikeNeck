using DiscriminantModel.PostureEstimates;
using System.Reflection;
using Image = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24>;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;
using NewFLD;
using Microsoft.ML;

namespace DiscriminantModel.ForwardLeanDetector
{
    class ForwardLeanDetectionTrainer
    {

        internal void retrain(DirectoryInfo currectPosture, DirectoryInfo forwardLeanPosture)
        {
            var trainDataPath = this.buildTrainData(currectPosture, forwardLeanPosture);
            var modelPath = MLModel1.MLNetModelPath;
            var context = new MLContext();
            var inputData = context.Data.LoadFromTextFile<MLModel1.ModelInput>(trainDataPath, separatorChar: ',', hasHeader: true);
            var newModel = MLModel1.RetrainPipeline(context, inputData);
            context.Model.Save(newModel, inputData.Schema, modelPath);
        }

        string buildTrainData(DirectoryInfo currectPosture, DirectoryInfo forwardLeanPosture)
        {
            var sb = new StringBuilder();

            var TRAIN_PATH = ForwardLeanDetectionResource.TRAIN_CSV_FILE;
            var sw = new StreamWriter(TRAIN_PATH, true);


            sb.AppendLine("N_LEar_X,N_LEar_Y,REar_REye_X,REar_REye_Y,LEye_LEar_X,LEye_LEar_Y,REye_N_X,REye_N_Y,N_LEye_X,N_+Eye_Y,FLD");

            var currentInputs = this.posturesEstimate(currectPosture);
            var currentTrainData = this.buildTrainDataAsString(currentInputs, false);
            sb.Append(currentTrainData);

            var forwardLeanInputs = this.posturesEstimate(forwardLeanPosture);
            var forwardLeanTrainData = this.buildTrainDataAsString(forwardLeanInputs, true);
            sb.Append(forwardLeanTrainData);

            var trainData = sb.ToString();
            sw.Write(trainData);
            sw.Flush();
            sw.Close();

            return TRAIN_PATH;
        }

        StringBuilder buildTrainDataAsString(List< Dictionary<InputType, float> > inputs, bool fld) 
        {
            var sb = new StringBuilder();

            foreach(var item in inputs)
            {
                sb.Append($"{item[InputType.N_LEar_X]},{item[InputType.N_LEar_Y]},{item[InputType.REar_REye_X]},{item[InputType.REar_REye_Y]}");
                sb.Append($"{item[InputType.LEye_LEar_X]},{item[InputType.LEye_LEar_Y]},{item[InputType.REye_N_X]},{item[InputType.REye_N_Y]}");
                sb.Append($"{item[InputType.N_LEye_X]},{item[InputType.N_LEye_Y]},");
                sb.AppendLine($"{fld}");
            }

            return sb;
        }

        List< Dictionary<InputType, float> > posturesEstimate(DirectoryInfo posture)
        {
            var inputs = new List< Dictionary<InputType, float> >();

            foreach (var sample in posture.GetFiles())
            {
                var input = this.postureEstimate(sample);
                inputs.Add(input);
            }

            return inputs;
        }   

        Dictionary<InputType, float> postureEstimate(FileInfo sample)
        {
            var postureEstimator = new PostureEstimatesAPI();
            var sampleImage = Image.Load<Rgb24>(sample.FullName);

            var result = postureEstimator.predict(sampleImage);

            Dictionary<InputType, float> input = new Dictionary<InputType, float>();

            input[InputType.N_LEye_X] = result[KeyPointType.Nose].x - result[KeyPointType.LeftEye].x;
            input[InputType.N_LEye_Y] = result[KeyPointType.Nose].y - result[KeyPointType.LeftEye].y;
            input[InputType.REye_N_X] = result[KeyPointType.RightEye].x - result[KeyPointType.Nose].x;
            input[InputType.REye_N_Y] = result[KeyPointType.RightEye].y - result[KeyPointType.Nose].y;
            input[InputType.LEye_LEar_X] = result[KeyPointType.LeftEye].x - result[KeyPointType.LeftEar].x;
            input[InputType.LEye_LEar_Y] = result[KeyPointType.LeftEye].y - result[KeyPointType.LeftEar].y;
            input[InputType.REar_REye_X] = result[KeyPointType.RightEar].x - result[KeyPointType.RightEye].x;
            input[InputType.REar_REye_Y] = result[KeyPointType.RightEar].y - result[KeyPointType.RightEye].y;
            input[InputType.N_LEar_X] = result[KeyPointType.Nose].x - result[KeyPointType.LeftEar].x;
            input[InputType.N_LEar_Y] = result[KeyPointType.Nose].y - result[KeyPointType.LeftEar].y;

            return input;
        }
    }
}
