using Camera.MAUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                await cameraAccessor.TakePhotoAsync();
                await Task.Delay(interval);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
