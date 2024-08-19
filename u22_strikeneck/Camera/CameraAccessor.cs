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
            using var imageStream = await cameraView.TakePhotoAsync(ImageFormat.PNG);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await imageStream.CopyToAsync(fileStream);
            return new FileInfo(filePath);
        }

        public async Task LoadCamera(string cameraDeviceName)
        {
            var usedCamera = cameraView.Cameras.FirstOrDefault(camera => camera.Name == cameraDeviceName);
            if (usedCamera == null)
                throw new CameraNotFoundException($"指定されたカメラ({cameraDeviceName})が見つかりませんでした");

            cameraView.Camera = usedCamera;
            await cameraView.StopCameraAsync();
            var result = await cameraView.StartCameraAsync();
            if (result != CameraResult.Success)
                throw new CameraInitializationException("カメラを正常に起動することができませんでした");
            await Task.Delay(250);
        }

        public async Task StopCameraAsync()
        {
            await cameraView.StopCameraAsync();
        }
    }
}
