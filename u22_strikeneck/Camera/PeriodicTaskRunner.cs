using ForwardLeanDetection.DiscriminantModel;
using System.Diagnostics;
using u22_strikeneck.AppSettingIO;

namespace u22_strikeneck.Camera
{
    internal class PeriodicTaskRunner
    {
        CameraAccessor cameraAccessor;
        TimeSpan interval = TimeSpan.FromSeconds(1);
        bool isRunning = false;


        public PeriodicTaskRunner(CameraAccessor cameraAccessor, TimeSpan interval)
        {
            this.cameraAccessor = cameraAccessor;
            this.interval = interval;
        }

        public async Task StartAsync()
        {
            if (isRunning) return;
            isRunning = true;
            await Task.Run( () => TaskFormat(Run) );
        }
         
        public void Stop()
        {
            isRunning = false;
        }

        private async Task TaskFormat(Func<Task> func)
        {
            var lastTime = DateTime.Now;
            while (isRunning)
            {
                Task.Delay(1000).Wait();
                var now = DateTime.Now;

                if (now - lastTime < interval)
                    continue;
                lastTime = now;
                await func();
            }
        }

        private async Task Run()
        {
            var fldAPI = new API();
            var dbWriter = new DatabaseWriter();
            var appSettingReader = new AppSettingReader();
            var toastSender = new PeriodicToastSender();

            var fileName = "image.png";
            var timeStamp = DateTime.Now;
            var bias = appSettingReader.GetDetectionSensitivity().value;

            var file = await cameraAccessor.TakePhotoAsync(fileName);
            var result = await fldAPI.Predict(file, bias);

            await dbWriter.UpdateOrInsertPostureEventAsync(timeStamp, result);

            if (result == true && toastSender.IsDurationPassed(timeStamp)) 
                await toastSender.sendToast();
        }
    }
}
