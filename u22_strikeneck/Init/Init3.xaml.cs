using Camera.MAUI;
using ForwardLeanDetection.DiscriminantModel;
using u22_strikeneck.Camera;
using u22_strikeneck.Camera.CameraException;
using ImageFormat = Camera.MAUI.ImageFormat;

namespace u22_strikeneck.Init;

public partial class Init3 : ContentPage
{
    private CameraAccessor cameraAccessor;
    private bool isTesting = false;
    private bool isRunningTest = false;
    private bool isInitialized = false;

    public Init3()
    {
        InitializeComponent();

        var rootDirectory = new InitDirectoryAccessor().RootDirectoryInfo;
        cameraAccessor = new CameraAccessor(cameraView, rootDirectory);
    }
    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        cameraLoad();
    }

    private void cameraLoad()
    {
        MainThread.BeginInvokeOnMainThread(async () => {
            var currentCameraName = new AppSettingIO.AppSettingReader().GetUsedCameraName();
            await cameraAccessor.LoadCamera(currentCameraName);
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            await ActivateFLDTest();
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isInitialized) return;
        cameraLoad();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        cameraAccessor.StopCameraAsync();
        isInitialized = true;
    }

    private async Task ActivateFLDTest()
    {
        isTesting = true;
        isRunningTest = true;
        var fld = new API();
        while (isTesting)
        {
            try
            {
                var fileInfo = await cameraAccessor.TakePhotoAsync("test.png");

                var isFLD = await fld.Predict(fileInfo);
                myImage.Source = ImageSource.FromFile(fileInfo.FullName);

                if (isFLD) FLDResult.Text = "ëOåXépê®Ç≈Ç∑";
                else FLDResult.Text = "ê≥ÇµÇ¢épê®Ç≈Ç∑";
            }catch(PhotoCaptureFailedException e)
            { }

            await Task.Delay(TimeSpan.FromMilliseconds(100));
        }
        isRunningTest = false;

    }

    private async void ToStats(object sender, EventArgs e)
    {
        isTesting = false;
        while(isRunningTest) await Task.Delay(TimeSpan.FromMilliseconds(100));
        await Shell.Current.GoToAsync("//Stats");
    }

    private async void ToInit1(object sender, EventArgs e)
    {
        isTesting = false;
        while (isRunningTest) await Task.Delay(TimeSpan.FromMilliseconds(100));
        await Shell.Current.GoToAsync("//Init1");
    }
    private async void ToInit2(object sender, EventArgs e)
    {
        isTesting = false;
        while (isRunningTest) await Task.Delay(TimeSpan.FromMilliseconds(100));
        await Shell.Current.GoToAsync("//Init2");
    }
}