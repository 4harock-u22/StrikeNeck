using u22_strikeneck.Camera;
using ImageFormat = Camera.MAUI.ImageFormat;

namespace u22_strikeneck.Init;

public partial class Init2 : ContentPage
{
    private CameraAccessor cameraAccessor;

    public Init2()
    {
        InitializeComponent();

        var forwardDirectoryInfo = new InitDirectoryAccessor().ForwardDirectoryInfo;
        cameraAccessor = new CameraAccessor(cameraView, forwardDirectoryInfo);
    }

    private async void ToInit3(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RetrainLoadingPage");
    }

    private async void ToInit1(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Init1");
    }

    private void TakePhotos(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            new InitDirectoryAccessor().ClearForwardDirectory();
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
        MainThread.BeginInvokeOnMainThread(
            async () => await cameraAccessor.LoadCamera()
        );
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(
            async () => await cameraAccessor.RestartCameraAsync()
       );
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MainThread.BeginInvokeOnMainThread(
            async () => await cameraAccessor.StopCameraAsync()
        );
    }
}
