

namespace u22_strikeneck.Camera.CameraException
{
    public class CameraInitializationException : CameraException
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
