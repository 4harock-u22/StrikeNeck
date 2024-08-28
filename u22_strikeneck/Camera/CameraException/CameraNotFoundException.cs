namespace u22_strikeneck.Camera.CameraException
{
    public class CameraNotFoundException : CameraException
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
