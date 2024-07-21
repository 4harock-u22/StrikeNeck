using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using Camera.MAUI; // Replace with actual CameraView namespace

namespace u22_strikeneck.Init
{
    public partial class Init1 : ContentPage
    {
        private TimeOnly StartTime = TimeOnly.FromDateTime(DateTime.Now);

        public Init1()
        {
            InitializeComponent();
            cameraView.CamerasLoaded += cameraView_CamerasLoaded;
        }

        private async void OnClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Init2");
        }

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                var result = await cameraView.StartCameraAsync();
            });
        }

        private async void TakePhoto(object sender, EventArgs e)
        {
            await cameraView.StopCameraAsync();
            var result = await cameraView.StartCameraAsync();

            List<string> imagePaths = new List<string>();
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "Photos");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            for (int i = 0; i < 50; i++)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                StartTime = TimeOnly.FromDateTime(DateTime.Now);

                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    using (var sourceStream = await photo.OpenReadAsync())
                    {
                        string filePath = Path.Combine(folderPath, $"photo_{i + 1}.png");
                        using (var fileStream = File.Create(filePath))
                        {
                            await sourceStream.CopyToAsync(fileStream);
                        }
                        imagePaths.Add(filePath);
                    }
                }

                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
            while (true)
            {

                await Task.Delay(TimeSpan.FromMilliseconds(10));
                StartTime = TimeOnly.FromDateTime(DateTime.Now);

                if (StartTime.Second % 5 == 0)
                {
                    myImage.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                    StartTime = TimeOnly.FromDateTime(DateTime.Now);
                }
            }


            // Use imagePaths as needed, e.g., passing to another function or returning from this function
        }



    }
}
