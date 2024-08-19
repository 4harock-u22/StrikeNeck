using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.AppSettingIO
{
    public class AppSettingWriter
    {
        public void UpdateNotificationStatus(bool status)
        {
            // Update notification status
            var key = AppSettingKey.NotificationStatus.ToString();
            Preferences.Default.Set(key, status);
        }

        public void UpdateNotificationInterval(NotificationInterval interval)
        {
            // Update notification interval
            var key = AppSettingKey.NotificationInterval.ToString();
            Preferences.Default.Set(key, (int)interval);
        }

        public void UpdateDetectionSensitivity(DetectionSensitivity sensitivity)
        {
            // Update detection sensitivity
            var key = AppSettingKey.DetectionSensitivity.ToString();
            Preferences.Default.Set(key, sensitivity.value);
        }

        public void UpdateUsedCameraName(string cameraName)
        {
            // Update used camera
            var key = AppSettingKey.UsedCamera.ToString();
            Preferences.Default.Set(key, cameraName);
        }
    }
}
