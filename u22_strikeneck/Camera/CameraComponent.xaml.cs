using Camera.MAUI;
using ImageFormat = Camera.MAUI.ImageFormat;

namespace u22_strikeneck.Camera;

public partial class CameraComponent : ContentView { 

    private CameraAccessor cameraAccessor;
    private PeriodicTaskRunner periodicTaskRunner;

    public CameraComponent()
    {
        InitializeComponent();

        var directoryPath = Path.Combine(FileSystem.Current.CacheDirectory, "local", "pic");
        var directoryInfo = new DirectoryInfo(directoryPath);
        if (! directoryInfo.Exists) directoryInfo.Create();
        
        var cameraAccessor = new CameraAccessor(cameraView, directoryInfo);
        this.cameraAccessor = cameraAccessor;

        periodicTaskRunner = new PeriodicTaskRunner(cameraAccessor, TimeSpan.FromSeconds(60));
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.NumCamerasDetected == 0) throw new Exception("カメラを検知できませんでした");

        cameraView.Camera = cameraView.Cameras.First();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            var result = await cameraView.StartCameraAsync();
            if (result != CameraResult.Success)
                throw new Exception("カメラを正常に起動することができませんでした");
            await Task.Delay(250);
            await periodicTaskRunner.StartAsync();
        });
    }    
}