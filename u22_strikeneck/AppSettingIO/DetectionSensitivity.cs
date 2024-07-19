using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.AppSettingIO
{
    public class DetectionSensitivity
    {
        private static readonly int MAX_SENSITIVITY = 100;
        private static readonly int MIN_SENSITIVITY = -100;

        private readonly int sensitivity;

        public DetectionSensitivity(int sensitivity = 0)
        {
            if (sensitivity < MIN_SENSITIVITY || sensitivity > MAX_SENSITIVITY)
                throw new ArgumentOutOfRangeException($"sensitivity must be between {MIN_SENSITIVITY} and ${MAX_SENSITIVITY}");

            this.sensitivity = sensitivity;
        }

        public int Sensitivity => sensitivity;
    }
}
