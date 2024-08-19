using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.Camera
{
    public class CameraNotFoundException : Exception
    {
        public CameraNotFoundException()
            : base("The specified camera could not be found.")
        {
        }

        public CameraNotFoundException(string message)
            : base(message)
        {
        }

        public CameraNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
