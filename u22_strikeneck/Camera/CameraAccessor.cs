using Camera.MAUI;
using ImageFormat = Camera.MAUI.ImageFormat;

namespace u22_strikeneck.Camera
{
    internal class CameraAccessor {

        private CameraView cameraView;
        private DirectoryInfo savedDirectory;
        internal CameraAccessor(CameraView cameraView, DirectoryInfo savedDirectory)
        {
            this.cameraView = cameraView;
            this.savedDirectory = savedDirectory;
        }

        internal FileInfo TakePhotoAsync(String fileName="image.png")
        {
            var filePath = Path.Combine(savedDirectory.FullName, fileName);
            var resTask = cameraView.SaveSnapShot(ImageFormat.PNG, filePath);
            resTask.Wait();
            return new FileInfo(filePath);
        }

        internal FileInfo TakePhoto(String fileName = "image.png")
        {
            var filePath = Path.Combine(savedDirectory.FullName, fileName);
            cameraView.SaveSnapShot(ImageFormat.PNG, filePath);
            return new FileInfo(filePath);
        }

        internal async Task LoadCamera()
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
