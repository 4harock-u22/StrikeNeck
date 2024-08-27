namespace u22_strikeneck.Camera.CameraException
{
    public class CameraException : Exception
    {
        public CameraException()
            : base("An exception was thrown in the processing related to the camera.")
        {
        }

        public CameraException(string message)
            : base(message)
        {
        }

        public CameraException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
