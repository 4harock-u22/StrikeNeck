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

        periodicTaskRunner = new PeriodicTaskRunner(cameraAccessor, TimeSpan.FromSeconds(3));
    }

    public CameraSelector GetCameraSelector()
    {
        return new CameraSelector(cameraView);
    }

    public async void StartPeriodicTask()
    {
        var currentCameraName = new AppSettingIO.AppSettingReader().GetUsedCameraName();
        cameraAccessor.LoadCamera(currentCameraName);
        periodicTaskRunner.StartAsync();
    }

    public void StopPeriodicTask()
    {
        periodicTaskRunner.Stop();
        cameraAccessor.StopCameraAsync();
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var currentCameraName = new AppSettingIO.AppSettingReader().GetUsedCameraName();
            await cameraAccessor.LoadCamera(currentCameraName);
            periodicTaskRunner.StartAsync();
        });
    }    

    
}