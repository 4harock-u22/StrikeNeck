using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Camera.MAUI;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SQLite;

namespace u22_strikeneck
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseSkiaSharp(true)
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).UseMauiCommunityToolkit();

            SQLitePCL.Batteries.Init();

            SQLitePCL.Batteries_V2.Init();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}