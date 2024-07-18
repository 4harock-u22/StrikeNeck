using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using LiveCharts.Wpf;
using u22_strikeneck.ViewModels;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

using System.IO;


namespace u22_strikeneck

{
    public class AnalyticsResult
    {
        public float[] ActivateTimes { get; set; }
        public float[] ForwardLeanTimes { get; set; }
        public string[] AxisLabels { get; set; }
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
        }

        private void DurationPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker == null)
            {
                File.AppendAllText("C://Log//log.txt", "On Main\n");
                return;
            }
            int selectedIndex = picker.SelectedIndex;
            UpdateAnalytics(selectedIndex);
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker == null)
            {
                File.AppendAllText("C://Log//log.txt", "On Back\n");
                return;
            }
            int selectedIndex = picker.SelectedIndex;
            UpdateAnalytics(selectedIndex, isBack: true);
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker == null)
            {
                File.AppendAllText("C://Log//log.txt", "On Next\n");
                return;
            }
            int selectedIndex = picker.SelectedIndex;
            File.AppendAllText("C://Log//log.txt", selectedIndex.ToString());
            UpdateAnalytics(selectedIndex, isNext: true);
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
                    return ProcessDailyAnalyticsData(analyticsPerDay);
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

        private AnalyticsResult ProcessDailyAnalyticsData(IEnumerable<AnalyticsData> analyticsData)
        {
            int length = 24;
            var result = new AnalyticsResult
            {
                ActivateTimes = new float[length],
                ForwardLeanTimes = new float[length],
                AxisLabels = analyticsData.Take(length)
                                          .Select(apd => $"{apd.Date.Hour}:00")
                                          .ToArray()
            };

            for (int i = 0; i < length; i++)
            {
                var data = analyticsData.ElementAt(i);
                result.ActivateTimes[i] = data.ActiveTime;
                result.ForwardLeanTimes[i] = data.ForwardLeanTime;
            }

            return result;
        }

        private AnalyticsResult ProcessWeeklyOrMonthlyAnalyticsData(IEnumerable<AnalyticsData> analyticsData, int length)
        {
            var result = new AnalyticsResult
            {
                ActivateTimes = new float[length],
                ForwardLeanTimes = new float[length],
                AxisLabels = new string[length]
            };

            {
                int previousMonth = 0;
                int index = 0;

                foreach (var data in analyticsData.Take(length))
                {
                    result.ActivateTimes[index] = data.ActiveTime;
                    result.ForwardLeanTimes[index] = data.ForwardLeanTime;

                    if (previousMonth != data.Date.Month)
                    {
                        result.AxisLabels[index] = $"{data.Date.Month.ToString().PadLeft(2, ' ')}/{data.Date.Day.ToString().PadLeft(2, ' ')}";
                        previousMonth = data.Date.Month;
                    }
                    else
                    {
                        result.AxisLabels[index] = $"{data.Date.Day.ToString().PadLeft(2, ' ')}";
                    }

                    index++;
                }
            }

            return result;
        }

        private void UpdateStatsViewModel(float[] activateTimes, float[] forwardLeanTimes, string[] axisLabels)
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