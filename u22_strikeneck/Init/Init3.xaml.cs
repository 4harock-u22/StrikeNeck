using Camera.MAUI;
using ForwardLeanDetection.DiscriminantModel;
using u22_strikeneck.Camera;
using ImageFormat = Camera.MAUI.ImageFormat;

namespace u22_strikeneck.Init;

public partial class Init3 : ContentPage
{
    private CameraAccessor cameraAccessor;

    public Init3()
    {
        InitializeComponent();

        var rootDirectory = new InitDirectoryAccessor().RootDirectoryInfo;
        cameraAccessor = new CameraAccessor(cameraView, rootDirectory);
    }
    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread( async () => {
            await cameraAccessor.LoadCamera();
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            await ActivateFLDTest();
        });
    }

    private async Task ActivateFLDTest()
    {
        var fld = new API();
        while (true)
        {
            var fileInfo = await cameraAccessor.TakePhotoAsync("test.png");

            var isFLD = await fld.Predict(fileInfo);
            myImage.Source = ImageSource.FromFile(fileInfo.FullName);

            if (isFLD) FLDResult.Text = "ëOåXépê®Ç≈Ç∑";
            else FLDResult.Text = "ê≥ÇµÇ¢épê®Ç≈Ç∑";

            await Task.Delay(TimeSpan.FromMilliseconds(100));
        }

    }

    private async void ToStats(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Stats");
    }

    private async void ToInit1(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Init1");
    }
    private async void ToInit2(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Init2");
    }
}