using DiscriminantModel.ForwardLeanDetector;
using DiscriminantModel.PostureEstimates;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24>;


namespace ForwardLeanDetection.DiscriminantModel
{
    public class API
    {
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
            await Task.Run(async () => {
                var _fldAPI = new ForwardLeanDetectionAPI();
                await _fldAPI.Retrain(currectPosture, forwardLeanPosture);
            });
            
        }
    }
}
