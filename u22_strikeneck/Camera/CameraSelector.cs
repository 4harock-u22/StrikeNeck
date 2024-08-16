using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Camera.MAUI;

namespace u22_strikeneck.Camera
{
    internal class CameraSelector
    {
        private CameraView cameraView;

        internal CameraSelector(CameraView cameraView)
        {
            this.cameraView = cameraView;
        }

        internal List<String> getCamerasName()
        {
            List<String> camerasName = new List<String>();
            foreach (var camera in cameraView.Cameras)
            {
                camerasName.Add(camera.Name);
            }
            return camerasName;
        }

        internal void selectCamera(String name)
        {
            cameraView.Camera = cameraView.Cameras.First(camera => camera.Name == name);
        }

        internal bool existCamera(String name)
        {
            return cameraView.Cameras.Any(camera => camera.Name == name);
        }
    }
}
