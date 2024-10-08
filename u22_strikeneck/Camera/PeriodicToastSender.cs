﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Data;
using u22_strikeneck.AppSettingIO;

namespace u22_strikeneck.Camera
{
    internal class PeriodicToastSender
    {
        private static DateTime lastSentTime = DateTime.Now.AddDays(-1);
        private ToastSender toastSender = new ToastSender();


        public bool IsDurationPassed(DateTime dataTime)
        {
            var settingReader = new AppSettingReader();
            var intervalKey = settingReader.GetNotificationInterval();
            var interval = buildTimeSpanFromIntervalKey(intervalKey);
            var duration = dataTime - lastSentTime;
            return duration >= interval;
        }

        public bool IsEnabled()
        {
            var settingReader = new AppSettingReader();
            return settingReader.GetNotificationStatus();
        }
        public async Task sendToast()
        {
            await toastSender.SendToast("前傾姿勢になっています。姿勢を正してください！！");
            lastSentTime = DateTime.Now;
        }

        private TimeSpan buildTimeSpanFromIntervalKey(NotificationInterval intervalKey)
        {
            var interval = TimeSpan.FromMinutes((int)intervalKey);
            return interval;
        }

    }
}
