using DiscriminantModel.ForwardLeanDetector;
using DiscriminantModel.PostureEstimates;
using SixLabors.ImageSharp.PixelFormats;
using System.Reflection;
using Image = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24>;


namespace ForwardLeanDetection.DiscriminantModel
{
    public class API
    {
        private bool isRetraining = false;

        public async Task<bool> Predict(FileInfo fileInfo, double bias=0)
        {
            var result = false;
            await Task.Run(async () =>
            {

                var _peAPI = new PostureEstimatesAPI();
                var _fldAPI = new ForwardLeanDetectionAPI();

                var image = await Image.LoadAsync<Rgb24>(fileInfo.FullName);
                var jointPositions = await _peAPI.Predict(image);
                result = await _fldAPI.Predict(jointPositions, bias);
            });
            return result;
        }

        public async Task Retrain(DirectoryInfo currectPosture, DirectoryInfo forwardLeanPosture)
        {
            await Task.Run(() => {
                isRetraining = true;
                var _fldAPI = new ForwardLeanDetectionAPI();
                _fldAPI.Retrain(currectPosture, forwardLeanPosture).Wait();
                isRetraining = false;
            });
        }

        public bool IsRetraining
        {
            get { return isRetraining; }
        }
    }
}
