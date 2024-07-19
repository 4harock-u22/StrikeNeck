using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Data;
using u22_strikeneck.AppSettingIO;

namespace u22_strikeneck.Camera
{
    internal class PeriodicToastSender
    {
        private static DateTime lastSentTime = DateTime.Now.AddDays(-1);
        private bool isRunning = false;


        public bool IsDurationPassed(DateTime dataTime)
        {
            var settingReader = new AppSettingReader();
            var intervalKey = settingReader.GetNotificationInterval();
            var interval = buildTimeSpanFromIntervalKey(intervalKey);
            var duration = dataTime - lastSentTime;
            return duration >= interval;
        }
        public async Task sendToast()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await Toast.Make("前傾姿勢になっています。姿勢を正してください！！",
                      ToastDuration.Short,
                      16)
                .Show(cancellationTokenSource.Token);

            lastSentTime = DateTime.Now;
        }

        private TimeSpan buildTimeSpanFromIntervalKey(NotificationInterval intervalKey)
        {
            var interval = TimeSpan.FromMinutes((int)intervalKey);
            return interval;
        }

    }
}
