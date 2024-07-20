using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.AppSettingIO
{
    public class DetectionSensitivity
    {
        private static readonly double MAX_SENSITIVITY = 1;
        private static readonly double MIN_SENSITIVITY = -1;

        public readonly double value;

        public DetectionSensitivity(double sensitivity = 0)
        {
            if (sensitivity < MIN_SENSITIVITY || sensitivity > MAX_SENSITIVITY)
                throw new ArgumentOutOfRangeException($"sensitivity must be between {MIN_SENSITIVITY} and ${MAX_SENSITIVITY}");

            this.value = sensitivity;
        }
    }
}
