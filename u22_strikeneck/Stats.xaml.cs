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
            DurationPicker.SelectedIndex = 1;
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
            DateTime date = new DateTime(2024, 7, 17);

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
            myLabel.Text = selectedIndex == 0 ? "(分)" : "(時間)";

            StatsViewModel.Series = StatsViewModel.Series;
            StatsViewModel.XAxes = StatsViewModel.XAxes;
            StatsViewModel.UpdateGraph();
        }

        private async Task<AnalyticsResult> GetAnalytics(int selectedIndex, DateTime date)
        {
            switch (selectedIndex)
            {
                case 0:
                    /*
                    var analyticsPerDay = await StatisticsProvider.GetAnalyticsPerDayAsync(date);
                    return ProcessDailyAnalyticsData(analyticsPerDay, 24);
                    */
                    var testData1 = new AnalyticsResult
                    {
                        ActivateTimes = new List<float>(new float[24] { 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f, 2.1f, 2.2f, 2.3f, 2.4f, 2.5f, 2.6f, 2.7f, 2.8f, 2.9f, 3.0f, 3.1f, 3.2f, 3.3f, 3.4f }),
                        ForwardLeanTimes = new List<float>(new float[24] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f, 2.1f, 2.2f, 2.3f, 2.4f }),
                        AxisLabels = new List<string>(new string[24] { "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "A10", "A11", "A12", "A13", "A14", "A15", "A16", "A17", "A18", "A19", "A20", "A21", "A22", "A23", "A24" })
                    };
                    return testData1;
                case 1:
                    /*
                    var analyticsPerWeek = await StatisticsProvider.GetAnalyticsPerWeekAsync(date);
                    return ProcessWeeklyOrMonthlyAnalyticsData(analyticsPerWeek, 7);
                    */
                    var testData2 = new AnalyticsResult
                    {
                        ActivateTimes = new List<float>(new float[7] { 10.1f, 10.2f, 10.3f, 10.4f, 10.5f, 10.6f, 10.7f }),
                        ForwardLeanTimes = new List<float>(new float[7] { 5.1f, 5.2f, 5.3f, 5.4f, 5.5f, 5.6f, 5.7f }),
                        AxisLabels = new List<string>(new string[7] { "B1", "B2", "B3", "B4", "B5", "B6", "B7" })
                    };
                    return testData2;
                case 2:
                    /*
                    var analyticsPerMonth = await StatisticsProvider.GetAnalyticsPerMonthAsync(date);
                    return ProcessWeeklyOrMonthlyAnalyticsData(analyticsPerMonth, 30);
                    */
                    var testData3 = new AnalyticsResult
                    {
                        ActivateTimes = new List<float>(new float[30] { 3.1f, 3.2f, 3.3f, 3.4f, 3.5f, 3.6f, 3.7f, 3.8f, 3.9f, 4.0f, 4.1f, 4.2f, 4.3f, 4.4f, 4.5f, 4.6f, 4.7f, 4.8f, 4.9f, 5.0f, 5.1f, 5.2f, 5.3f, 5.4f, 5.5f, 5.6f, 5.7f, 5.8f, 5.9f, 6.0f }),
                        ForwardLeanTimes = new List<float>(new float[30] { 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f, 2.1f, 2.2f, 2.3f, 2.4f, 2.5f, 2.6f, 2.7f, 2.8f, 2.9f, 3.0f, 3.1f, 3.2f, 3.3f, 3.4f, 3.5f, 3.6f, 3.7f, 3.8f, 3.9f }),
                        AxisLabels = new List<string>(new string[30] { "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C12", "C13", "C14", "C15", "C16", "C17", "C18", "C19", "C20", "C21", "C22", "C23", "C24", "C25", "C26", "C27", "C28", "C29", "C30" })
                    };
                    return testData3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private AnalyticsResult ProcessDailyAnalyticsData(IEnumerable<AnalyticsData> analyticsData, int length)
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