using ForwardLeanDetection.DiscriminantModel;

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
            while (isRunning)
            {
                var fldAPI = new API();
                var dbWriter = new DatabaseWriter();
                var timestamp = DateTime.Now;
                var file = await cameraAccessor.TakePhotoAsync();

                var result = fldAPI.Predict(file, 0);
                
                await dbWriter.UpdateOrInsertPostureEventAsync(timestamp, result);

                await Task.Delay(interval);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
