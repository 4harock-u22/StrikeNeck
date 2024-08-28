using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.Camera.CameraException
{
    public class PhotoCaptureFailedException : Exception
    {
        public PhotoCaptureFailedException()
            : base("Failed to capture the photo.")
        {
        }

        public PhotoCaptureFailedException(string message)
            : base(message)
        {
        }

        public PhotoCaptureFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
