using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.AppSettingIO
{
    public class AppSettingReader
    {
        public AppSettingReader()
        {
            if (!Preferences.Default.ContainsKey(AppSettingKey.NotificationStatus.ToString()))
                Preferences.Default.Set(AppSettingKey.NotificationStatus.ToString(), true);

            if (!Preferences.Default.ContainsKey(AppSettingKey.NotificationInterval.ToString()))
                Preferences.Default.Set(AppSettingKey.NotificationInterval.ToString(), (int)NotificationInterval.FifteenMinutes);

            if (!Preferences.Default.ContainsKey(AppSettingKey.DetectionSensitivity.ToString()))
                Preferences.Default.Set(AppSettingKey.DetectionSensitivity.ToString(), 0d);
        }

        public bool GetNotificationStatus()
        {
            var key = AppSettingKey.NotificationStatus.ToString();

            return Preferences.Default.Get(key, true);
        }

        public NotificationInterval GetNotificationInterval()
        {
            var key = AppSettingKey.NotificationInterval.ToString();
            return Preferences.Default.Get(key, NotificationInterval.FifteenMinutes);
        }

        public DetectionSensitivity GetDetectionSensitivity()
        {
            var key = AppSettingKey.DetectionSensitivity.ToString();
            return new DetectionSensitivity(Preferences.Default.Get(key, 0d));
        }


    }
}
