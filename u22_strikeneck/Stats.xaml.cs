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
    public class StatsData
    {
        public List<float> ActivateTimes { get; set; }
        public List<float> ForwardLeanTimes { get; set; }
        public List<string> AxisLabels { get; set; }
    }

    public partial class Stats : ContentPage
    {
        private StatsViewModel statsViewModel;
        DateTime date;
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
            date = DateTime.Now;
            DurationPicker.SelectedIndex = 0;
        }

        private async void ChangSelectIndex(object sender, EventArgs e)
        {
            await UpdateAnalytics(DurationPicker.SelectedIndex);
        }

        private async void ClickBackButton(object sender, EventArgs e)
        {
            await UpdateAnalytics(DurationPicker.SelectedIndex, isBack: true);
        }

        private async void ClickNextButton(object sender, EventArgs e)
        {
            await UpdateAnalytics(DurationPicker.SelectedIndex, isNext: true);
        }

        private async Task UpdateAnalytics(int selectedIndex, bool isBack = false, bool isNext = false)
        {
            Label myLabel = this.FindByName<Label>("unit");

            if (isBack)
            {
                switch (selectedIndex)
                {
                    case 0: date = date.AddDays(-1); break;
                    case 1: date = date.AddDays(-7); break;
                    case 2: date = date.AddMonths(-1); break;
                }
            }
            else if (isNext)
            {
                switch (selectedIndex)
                {
                    case 0: date = date.AddDays(1); break;
                    case 1: date = date.AddDays(7); break;
                    case 2: date = date.AddMonths(1); break;
                }
            }

            var analytics = await GetAnalytics(selectedIndex, date);

            UpdateStatsViewModel(analytics.ActivateTimes, analytics.ForwardLeanTimes, analytics.AxisLabels);
            myLabel.Text = "(分)";

            StatsViewModel.Series = StatsViewModel.Series;
            StatsViewModel.XAxes = StatsViewModel.XAxes;
            StatsViewModel.UpdateGraph();
        }

        private async Task<StatsData> GetAnalytics(int selectedIndex, DateTime date)
        {
            switch (selectedIndex)
            {
                case 0:
                    return await StatisticsProvider.ProcessDailyAnalyticsData(date, 24);
                case 1:
                    return await StatisticsProvider.ProcessWeeklyAnalyticsData(date, 7);
                case 2:
                    return await StatisticsProvider.ProcessMonthlyAnalyticsData(date, 30);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateStatsViewModel(List<float> activateTimes, List<float> forwardLeanTimes, List<string> axisLabels)
        {
            StatsViewModel.SetStartUpTime(activateTimes);
            StatsViewModel.SetPoorPostureTime(forwardLeanTimes);
            StatsViewModel.SetAxisLabels(axisLabels);
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

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}