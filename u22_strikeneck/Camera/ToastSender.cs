using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace u22_strikeneck.Camera
{
    internal class ToastSender
    {
        internal async Task SendToast(string message)
        {

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await Toast.Make(message,
                      ToastDuration.Short,
                          16)
                    .Show(cancellationTokenSource.Token);
        }
    }
}
