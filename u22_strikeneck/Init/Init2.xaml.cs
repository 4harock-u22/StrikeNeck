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

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(
            async () => await cameraAccessor.LoadCamera()
        );
    }

    private async void ToInit3(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Init3");
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
            for (int i = 0; i < 50; i++)
            {
                await cameraAccessor.LoadCamera();
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                await cameraAccessor.TakePhotoAsync($"photo_{i + 1}.png");
            }
        });
    }
}
