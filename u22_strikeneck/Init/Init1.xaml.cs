using u22_strikeneck.Camera;
using ImageFormat = Camera.MAUI.ImageFormat;

namespace u22_strikeneck.Init
{
    public partial class Init1 : ContentPage
    {
        private CameraAccessor cameraAccessor;

        public Init1()
        {
            InitializeComponent();

            var savedDirectoryInfo = new InitDirectoryAccessor().CorrectDirectoryInfo;

            cameraAccessor = new CameraAccessor(cameraView, savedDirectoryInfo);
        }

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(
                async () => await cameraAccessor.LoadCamera()
            );
        }

        private async void OnClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Init2");
        }

        private void TakePhotos(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                new InitDirectoryAccessor().ClearCorrectDirectory();
                for (int i = 0; i < 50; i++)
                {
                    await cameraAccessor.LoadCamera();
                    
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
                    var takenPhoto = await cameraAccessor.TakePhotoAsync($"photo_{i + 1}.png");
                    myImage.Source = ImageSource.FromFile(takenPhoto.FullName);
                }
            });
        }
    }
}
