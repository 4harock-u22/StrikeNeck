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
    public class AnalyticsResult
    {
        public List<float> ActivateTimes { get; set; }
        public List<float> ForwardLeanTimes { get; set; }
        public List<string> AxisLabels { get; set; }
    }

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

        private void ChangSelectIndex(object sender, EventArgs e)
        {
            UpdateAnalytics(DurationPicker.SelectedIndex);
        }

        private void ClickBackButton(object sender, EventArgs e)
        {
            UpdateAnalytics(DurationPicker.SelectedIndex, isBack: true);
        }

        private void ClickNextButton(object sender, EventArgs e)
        {
            UpdateAnalytics(DurationPicker.SelectedIndex, isNext: true);
        }

        private void UpdateAnalytics(int selectedIndex, bool isBack = false, bool isNext = false)
        {
            Label myLabel = this.FindByName<Label>("unit");
            DateTime date = DateTime.Now;

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

            var analytics = GetAnalytics(selectedIndex, date);

            UpdateStatsViewModel(analytics.ActivateTimes, analytics.ForwardLeanTimes, analytics.AxisLabels);
            myLabel.Text = selectedIndex == 0 ? "(分)" : "(時間)";

            StatsViewModel.Series = StatsViewModel.Series;
            StatsViewModel.XAxes = StatsViewModel.XAxes;
            StatsViewModel.UpdateGraph();
        }

        private AnalyticsResult GetAnalytics(int selectedIndex, DateTime date)
        {
            switch (selectedIndex)
            {
                case 0:
                    var analyticsPerDay = StatisticsProvider.GetAnalyticsPerDayAsync(date).Result;
                    return ProcessDailyAnalyticsData(analyticsPerDay, 24);
                case 1:
                    var analyticsPerWeek = StatisticsProvider.GetAnalyticsPerWeekAsync(date).Result;
                    return ProcessWeeklyOrMonthlyAnalyticsData(analyticsPerWeek, 7);
                case 2:
                    var analyticsPerMonth = StatisticsProvider.GetAnalyticsPerMonthAsync(date).Result;
                    return ProcessWeeklyOrMonthlyAnalyticsData(analyticsPerMonth, 30);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private AnalyticsResult ProcessDailyAnalyticsData(IEnumerable<AnalyticsData> analyticsData, int length = 24)
        {
            var result = new AnalyticsResult
            {
                ActivateTimes = analyticsData.Take(length).Select(apd => apd.ActiveTime).ToList(),
                ForwardLeanTimes = analyticsData.Take(length).Select(apd => apd.ForwardLeanTime).ToList(),
                AxisLabels = analyticsData.Take(length)
                                          .Select(apd => $"{apd.Date.Hour}:00")
                                          .ToList()
            };

            return result;
        }

        private AnalyticsResult ProcessWeeklyOrMonthlyAnalyticsData(IEnumerable<AnalyticsData> analyticsData, int length)
        {
            var result = new AnalyticsResult
            {
                ActivateTimes = analyticsData.Take(length).Select(apwm => apwm.ActiveTime).ToList(),
                ForwardLeanTimes = analyticsData.Take(length).Select(apwm => apwm.ForwardLeanTime).ToList(),
                AxisLabels = new List<string>()
            };

            {
                int previousMonth = 0;
                int index = 0;

                foreach (var data in analyticsData.Take(length))
                {
                    result.ActivateTimes.Add(data.ActiveTime);
                    result.ForwardLeanTimes.Add(data.ForwardLeanTime);

                    if (previousMonth != data.Date.Month)
                    {
                        result.AxisLabels.Add($"{data.Date.Month.ToString().PadLeft(2, ' ')}/{data.Date.Day.ToString().PadLeft(2, ' ')}");
                        previousMonth = data.Date.Month;
                    }
                    else
                    {
                        result.AxisLabels.Add($"{data.Date.Day.ToString().PadLeft(2, ' ')}");
                    }

                    index++;
                }
            }

            return result;
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