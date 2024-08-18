using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using u22_strikeneck.AppSettingIO;
using Camera.MAUI;

namespace u22_strikeneck.Camera
{
    public class CameraSelector
    {
        private CameraView cameraView;

        internal string convertCameraNameToDeviceId(String name)
        {
            return cameraView.Cameras.First(camera => camera.Name == name).DeviceId;
        }

        internal CameraSelector(CameraView cameraView)
        {
            this.cameraView = cameraView;
        }

        internal List<String> getCameraNames()
        {
            List<String> camerasName = new List<String>();
            foreach (var camera in cameraView.Cameras)
            {
                camerasName.Add(camera.Name);
            }
            return camerasName;
        }

        internal string getCurrentCameraName()
        {
            return cameraView.Camera.Name;
        }

        internal void setUsedCameraFromId(String deviceId)
        {
            cameraView.Camera = cameraView.Cameras.First(camera => camera.DeviceId == deviceId);
        }

        internal void setUsedCameraFromName(String name)
        {
            cameraView.Camera = cameraView.Cameras.First(camera => camera.Name == name);
        }

        internal bool existCamera(String name)
        {
            return cameraView.Cameras.Any(camera => camera.Name == name);
        }
    }
}
