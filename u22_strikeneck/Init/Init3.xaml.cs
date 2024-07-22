using Camera.MAUI;
using u22_strikeneck.Camera;
using ImageFormat = Camera.MAUI.ImageFormat;

namespace u22_strikeneck.Init;

public partial class Init3 : ContentPage
{
    private TimeOnly StartTime = TimeOnly.FromDateTime(DateTime.Now);
    public Init3()
    {
        InitializeComponent();
    }
    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        cameraView.Camera = cameraView.Cameras.First();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            var result = await cameraView.StartCameraAsync();

        });
    }

    private async void TakePhoto(object sender, EventArgs e)
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            StartTime = TimeOnly.FromDateTime(DateTime.Now);

            if (StartTime.Second % 10 == 0)
            {
                StartTime = TimeOnly.FromDateTime(DateTime.Now);
                myImage.Source = cameraView.GetSnapShot(ImageFormat.PNG);
                await Task.Delay(TimeSpan.FromMilliseconds(1000));
                StartTime = TimeOnly.FromDateTime(DateTime.Now);
            }
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