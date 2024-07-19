using Camera.MAUI;
using ForwardLeanDetection.DiscriminantModel;
using Microsoft.Maui.Storage;
using System.Diagnostics;

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

            #if DEBUG
                var logFile = System.IO.File.OpenWrite(Path.Combine(FileSystem.CacheDirectory, "log.txt"));
                var traceListener = new TextWriterTraceListener(logFile);
                System.Diagnostics.Trace.AutoFlush = true;
                System.Diagnostics.Trace.Listeners.Add(traceListener);
            #endif
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

            var fileName = "image.png";
            var timeStamp = DateTime.Now;

            var file = cameraAccessor.TakePhotoAsync(fileName);
            var result = await fldAPI.Predict(file, 0);

            await dbWriter.UpdateOrInsertPostureEventAsync(timeStamp, result);

            #if DEBUG
                Trace.WriteLine("FLD: " + result);
            #endif
        }
    }
}
