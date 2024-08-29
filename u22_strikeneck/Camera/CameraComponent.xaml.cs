using Camera.MAUI;
using CommunityToolkit.Maui.Core;
using System.Threading;
using u22_strikeneck.Camera.CameraException;
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

    public CameraSelector GetCameraSelector()
    {
        return new CameraSelector(cameraView);
    }

    public async void StartPeriodicTask()
    {
        try
        {
            var currentCameraName = new AppSettingIO.AppSettingReader().GetUsedCameraName();
            cameraAccessor.LoadCamera(currentCameraName);
            if (cameraAccessor.IsLoaded) await periodicTaskRunner.StartAsync();
        }
        catch (CameraException.CameraException e)
        {
            await new ToastSender().SendToast("カメラの起動に失敗しました。設定を確認してください。");
        }
    }

    public void StopPeriodicTask()
    {
        periodicTaskRunner.Stop();
        Task.Delay(500);
        cameraAccessor.StopCameraAsync();
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var currentCameraName = new AppSettingIO.AppSettingReader().GetUsedCameraName();
                await cameraAccessor.LoadCamera(currentCameraName);
                if(cameraAccessor.IsLoaded) await periodicTaskRunner.StartAsync();
            }catch(CameraException.CameraException e)
            {
                await new ToastSender().SendToast("カメラの起動に失敗しました。設定を確認してください。");
            
            }
        });
    }    

    
}