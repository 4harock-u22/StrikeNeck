using DiscriminantModel.ForwardLeanDetector;
using DiscriminantModel.PostureEstimates;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24>;


namespace ForwardLeanDetection.DiscriminantModel
{
    public class API
    {
        public bool Predict(FileInfo fileInfo, double bias)
        {
            var _peAPI = new PostureEstimatesAPI();
            var _fldAPI = new ForwardLeanDetectionAPI();

            var image = Image.Load<Rgb24>(fileInfo.FullName);
            var jointPositions = _peAPI.predict(image);
            var result = _fldAPI.Predict(jointPositions, bias);

            return result;
        }

        public async void Retrain(DirectoryInfo currectPosture, DirectoryInfo forwardLeanPosture)
        {

            var _fldAPI = new ForwardLeanDetectionAPI();
            _fldAPI.Retrain(currectPosture, forwardLeanPosture);
        }
    }
}
