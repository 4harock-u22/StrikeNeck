using u22_strikeneck.Camera;
using ImageFormat = Camera.MAUI.ImageFormat;
using u22_strikeneck.Camera.CameraException;

namespace u22_strikeneck.Init
{
    public partial class Init1 : ContentPage
    {
        private CameraAccessor cameraAccessor;
        bool isInitialized = false;

        public Init1()
        {
            InitializeComponent();

            var savedDirectoryInfo = new InitDirectoryAccessor().CorrectDirectoryInfo;

            cameraAccessor = new CameraAccessor(cameraView, savedDirectoryInfo);
        }


        private async void OnClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Init2");
        }
        private async void ToStats(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Stats");
        }

        private void TakePhotos(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                new InitDirectoryAccessor().ClearCorrectDirectory();
                //await cameraAccessor.LoadCamera();
                for (int i = 0; i < 50; i++)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    var takenPhoto = await cameraAccessor.TakePhotoAsync($"photo_{i + 1}.png");
                    myImage.Source = ImageSource.FromFile(takenPhoto.FullName);
                }
            });
        }
        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                try
                {
                    var currentCameraName = new AppSettingIO.AppSettingReader().GetUsedCameraName();
                    cameraAccessor = new CameraAccessor(cameraView, new InitDirectoryAccessor().CorrectDirectoryInfo);
                    await cameraAccessor.LoadCamera(currentCameraName);
                }
                catch (CameraNotFoundException ex)
                {
                    await new ToastSender().SendToast("カメラを正常に起動できませんでした。設定を確認して下さい。"); 
                    await Shell.Current.GoToAsync("//Settings");
                }

            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(!isInitialized) return;
            MainThread.BeginInvokeOnMainThread(async () => {
                try
                {
                    var currentCameraName = new AppSettingIO.AppSettingReader().GetUsedCameraName();
                    await cameraAccessor.LoadCamera(currentCameraName);
                }
                catch (CameraNotFoundException ex)
                {
                    await new ToastSender().SendToast("カメラを正常に起動できませんでした。設定を確認してください。");
                    await Shell.Current.GoToAsync("//Settings");
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            cameraAccessor.StopCameraAsync();
            isInitialized = true;
        }
    }
}
