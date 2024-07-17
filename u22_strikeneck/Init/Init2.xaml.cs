namespace u22_strikeneck.Init;

public partial class Init2 : ContentPage
{
    private TimeOnly StartTime = TimeOnly.FromDateTime(DateTime.Now);
    public Init2()
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
        await cameraView.StopCameraAsync();
        var result = await cameraView.StartCameraAsync();
        while (true)
        {

            await Task.Delay(TimeSpan.FromMilliseconds(10));
            StartTime = TimeOnly.FromDateTime(DateTime.Now);

            if (StartTime.Second % 5 == 0)
            {
                myImage.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
                await Task.Delay(TimeSpan.FromMilliseconds(1000));
                StartTime = TimeOnly.FromDateTime(DateTime.Now);
            }
        }
    }


    private async void ToInit3(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Init3");
    }
    private async void ToInit1(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Init1");
    }
}