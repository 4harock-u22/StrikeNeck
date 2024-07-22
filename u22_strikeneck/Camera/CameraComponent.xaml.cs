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

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraAccessor.LoadCamera();
            await periodicTaskRunner.StartAsync();
        });
    }    

}