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

        internal async Task<FileInfo> TakePhotoAsync(String fileName="image.jpeg")
        {
            var filePath = Path.Combine(savedDirectory.FullName, fileName);
            var isSuccess = await cameraView.SaveSnapShot(ImageFormat.JPEG, filePath);
            return new FileInfo(filePath);
        }
    }
}
