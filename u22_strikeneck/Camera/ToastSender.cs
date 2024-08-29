using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace u22_strikeneck.Camera
{
    internal class ToastSender
    {
        internal async Task SendToast(string message)
        {
            var toastIsEnabled = new AppSettingIO.AppSettingReader().GetNotificationStatus();
            if (!toastIsEnabled) return;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await Toast.Make(message,
                      ToastDuration.Short,
                          16)
                    .Show(cancellationTokenSource.Token);
        }
    }
}
