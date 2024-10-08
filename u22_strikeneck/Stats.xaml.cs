using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using LiveCharts.Wpf;
using u22_strikeneck.ViewModels;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace u22_strikeneck
{
    public partial class Stats : ContentPage
    {
        private StatsViewModel statsViewModel;
        public StatsViewModel StatsViewModel
        {
            get { return statsViewModel; }
            set
            {
                statsViewModel = value;
                BindingContext = statsViewModel;
            }
        }
        public Stats()
        {
            InitializeComponent();
            Binding binding = new Binding();
            StatsViewModel = new StatsViewModel();
            DurationPicker.SelectedIndex = 0;
        }

        private async void ChangSelectIndex(object sender, EventArgs e)
        {
            StatsViewModel.SetSelectedIndex(DurationPicker.SelectedIndex);
            await StatsViewModel.UpdateStatsAsync();
        }

        private async void ClickBackButton(object sender, EventArgs e)
        {
            StatsViewModel.SetSelectedIndex(DurationPicker.SelectedIndex);
            await StatsViewModel.UpdateStatsAsync(isBack: true);
        }

        private async void ClickNextButton(object sender, EventArgs e)
        {
            StatsViewModel.SetSelectedIndex(DurationPicker.SelectedIndex);
            await StatsViewModel.UpdateStatsAsync(isNext: true);
        }

        private async void ClickResetDateButton(object sender, EventArgs e)
        {
            StatsViewModel.SetCurrentDate();
            await StatsViewModel.UpdateStatsAsync();
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Settings");
        }


        private async void MakeToast(object sender, EventArgs e)
        {

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.Red,
                TextColor = Colors.Green,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(10),
                Font = Microsoft.Maui.Font.SystemFontOfSize(14),
                ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
                CharacterSpacing = 0.5
            };

            string text = "テスト通知";
            string actionButtonText = "設定を開く";
            Action action = async () => await Shell.Current.GoToAsync("//Settings");
            TimeSpan duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(text, action, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            cameraComponent.StartPeriodicTask();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            cameraComponent.StopPeriodicTask();
        }
    }
}