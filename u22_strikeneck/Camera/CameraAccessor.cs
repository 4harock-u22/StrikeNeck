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
    }
}
