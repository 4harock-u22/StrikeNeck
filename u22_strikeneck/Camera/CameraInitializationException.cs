using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.Camera
{
    public class CameraInitializationException : Exception
    {
        public CameraInitializationException()
            : base("Failed to initialize the camera.")
        {
        }

        public CameraInitializationException(string message)
            : base(message)
        {
        }

        public CameraInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
