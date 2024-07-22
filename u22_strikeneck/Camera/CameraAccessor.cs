using Camera.MAUI;
using ImageFormat = Camera.MAUI.ImageFormat;

namespace u22_strikeneck.Camera
{
    public class CameraAccessor {

        private CameraView cameraView;
        private DirectoryInfo savedDirectory;
        public CameraAccessor(CameraView cameraView, DirectoryInfo savedDirectory)
        {
            this.cameraView = cameraView;
            this.savedDirectory = savedDirectory;
        }

        public async Task<FileInfo> TakePhotoAsync(String fileName="image.png")
        {
            var filePath = Path.Combine(savedDirectory.FullName, fileName);
            var resTask = await cameraView.SaveSnapShot(ImageFormat.PNG, filePath);
            return new FileInfo(filePath);
        }

        internal FileInfo TakePhoto(String fileName = "image.png")
        {
            var filePath = Path.Combine(savedDirectory.FullName, fileName);
            cameraView.SaveSnapShot(ImageFormat.PNG, filePath);
            return new FileInfo(filePath);
        }

        public async Task LoadCamera()
        {
            if (cameraView.NumCamerasDetected == 0) throw new Exception("カメラを検知できませんでした");

            cameraView.Camera = cameraView.Cameras.First();
            await cameraView.StopCameraAsync();
            var result = await cameraView.StartCameraAsync();
            if (result != CameraResult.Success)
                throw new Exception("カメラを正常に起動することができませんでした");
            await Task.Delay(250);
        }
    }
}
