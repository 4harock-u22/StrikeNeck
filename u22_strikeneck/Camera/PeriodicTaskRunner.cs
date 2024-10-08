﻿using ForwardLeanDetection.DiscriminantModel;
using System.Diagnostics;
using u22_strikeneck.AppSettingIO;
using u22_strikeneck.Camera.CameraException;

namespace u22_strikeneck.Camera
{
    internal class PeriodicTaskRunner
    {
        CameraAccessor cameraAccessor;
        TimeSpan interval = TimeSpan.FromSeconds(1);
        bool isRunning = false;
        bool isStopped = true;


        public bool IsRunning => isRunning;
        public PeriodicTaskRunner(CameraAccessor cameraAccessor, TimeSpan interval)
        {
            this.cameraAccessor = cameraAccessor;
            this.interval = interval;
        }

        public async Task StartAsync()
        {
            if (isRunning) return;
            isRunning = true;
            isStopped = false;
            await Task.Run( () => TaskFormat(Run) );
        }
         
        public void Stop()
        {
            if(!isRunning) return;

            isRunning = false;

            //while (!isStopped) ;
        }

        private async Task TaskFormat(Func<Task> func)
        {
            var lastTime = DateTime.Now;
            while (isRunning)
            {
                await Task.Delay(1000);
                if (!isRunning) break;
                var now = DateTime.Now;

                if (now - lastTime < interval)
                    continue;
                lastTime = now;
                await func();
            }
            isStopped = true;
        }

        private async Task Run()
        {
            try
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

                if (!result) return;
                if (!toastSender.IsDurationPassed(timeStamp)) return;
                if (!toastSender.IsEnabled()) return;
                await toastSender.sendToast();
            }
            catch (PhotoCaptureFailedException e)
            {
                await new ToastSender().SendToast("写真の撮影に失敗しました。カメラが利用可能かを確認してください。");
            }
        }
    }
}
