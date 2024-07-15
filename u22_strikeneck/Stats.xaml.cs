using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using LiveCharts.Wpf;
using u22_strikeneck.ViewModels;
using System;


namespace u22_strikeneck

{
    enum DayOfWeek
    {
        SUN, MON, TUE, WED, THU, FRI, SAT
    }

    public class WeekCountUtility
    {
        public static int GetWeekCount(DateTime targetDate)
        {
            // 指定した年と月の最初の日を取得
            DateTime firstDayOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);

            // 指定した年と月の最後の日を取得
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // 最初の日から最後の日までの日数を計算
            int totalDays = (int)(lastDayOfMonth - firstDayOfMonth).TotalDays + 1;

            // 週数を計算
            int weekCount = (int)Math.Ceiling((double)totalDays / 7);

            return weekCount;
        }
    }
    public class MonthUtility
    {
        public static int GetCurrentMonth()
        {
            // 現在の日時を取得し、その月の番号を返す
            return DateTime.Now.Month;
        }
    }

    public class DayOfWeekUtility
    {
        public static int GetCurrentDayOfWeek()
        {
            // 現在の日時を取得
            DateTime now = DateTime.Now;

            // 現在の曜日を取得し、0から6の数字に変換
            int dayOfWeek = (int)now.DayOfWeek;

            // 日曜から始まるように調整
            if (dayOfWeek == 0)
            {
                dayOfWeek = 6;
            }
            else
            {
                dayOfWeek--;
            }

            return dayOfWeek;
        }
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

        DateTime date = DateTime.Now;
        private void DurationPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedIndex = picker.SelectedIndex;
            Label myLabel = this.FindByName<Label>("unit");
            date = DateTime.Now;
            switch (selectedIndex)
            {
                case 0:
                    //var analyticsPerDay = StatisticsProvider.GetAnalyticsPerDay(date);
                    float[] dayActivateTimes = new float[24];
                    float[] dayForwardLeanTimes = new float[24];
                    for(int i = 0; i < 24; i++)
                    {
                        //dayActivateTimes[i] = analyticsPerDay[i].ActiveTime;
                        dayActivateTimes[i] = i;
                        //dayForwardLeanTimes[i] = analyticsPerDay[i].ForwardLeanTime;
                        dayForwardLeanTimes[i] = i;
                    }

                    StatsViewModel.SetStartUpTime(dayActivateTimes);

                    StatsViewModel.SetPoorPostureTime(dayForwardLeanTimes);
                    StatsViewModel.SetAxisLabels(new string[] {
                        $"{date.Month}/{date.Day} 0時",
                        "1時",
                        "2時",
                        "3時",
                        "4時",
                        "5時",
                        "6時",
                        "7時",
                        "8時",
                        "9時",
                        "10時",
                        "11時",
                        "12時",
                        "13時",
                        "14時",
                        "15時",
                        "16時",
                        "17時",
                        "18時",
                        "19",
                        "20時",
                        "21時",
                        "22時",
                        "23時",
                    });
                    myLabel.Text = "(分)";
                    break;
                case 1:
                    
                    //var analyticsPerWeek = StatisticsProvider.GetAnalyticsPerWeek(date);
                    int sun = (int)DayOfWeek.SUN;
                    int mon = (int)DayOfWeek.MON;
                    int tue = (int)DayOfWeek.TUE;
                    int wed = (int)DayOfWeek.WED;
                    int thu = (int)DayOfWeek.THU;
                    int fri = (int)DayOfWeek.FRI;
                    int sat = (int)DayOfWeek.SAT;

                    //float ActivateSun = analyticsPerWeek[sun].ActiveTime;
                    //float ActivateMon = analyticsPerWeek[mon].ActiveTime;
                    //float ActivateTue = analyticsPerWeek[tue].ActiveTime;
                    //float ActivateWed = analyticsPerWeek[wed].ActiveTime;
                    //float ActivateThu = analyticsPerWeek[thu].ActiveTime;
                    //float ActivateFri = analyticsPerWeek[fri].ActiveTime;
                    //float ActivateSat = analyticsPerWeek[sat].ActiveTime;

                    //float fowardLeanSun = analyticsPerWeek[sun].ForwardLeanTime;
                    //float fowardLeanMon = analyticsPerWeek[mon].ForwardLeanTime;
                    //float fowardLeanTue = analyticsPerWeek[tue].ForwardLeanTime;
                    //float fowardLeanWed = analyticsPerWeek[wed].ForwardLeanTime;
                    //float fowardLeanThu = analyticsPerWeek[fri].ForwardLeanTime;
                    //float fowardLeanFri = analyticsPerWeek[sat].ForwardLeanTime;
                    //float fowardLeanSat = analyticsPerWeek[sun].ForwardLeanTime;

                    float ActivateSun = 1;
                    float ActivateMon = 2;
                    float ActivateTue = 8;
                    float ActivateWed = 34;
                    float ActivateThu = 3;
                    float ActivateFri = 12;
                    float ActivateSat = 33;

                    float fowardLeanSun = 43;
                    float fowardLeanMon = 3;
                    float fowardLeanTue = 44;
                    float fowardLeanWed = 54;
                    float fowardLeanThu = 56;
                    float fowardLeanFri = 12;
                    float fowardLeanSat = 23;



                    StatsViewModel.SetStartUpTime(new float[] {
                        ActivateSun,
                        ActivateMon,
                        ActivateTue,
                        ActivateWed,
                        ActivateThu,
                        ActivateFri,
                        ActivateSat,
                        });
                    StatsViewModel.SetPoorPostureTime(new float[] {
                        fowardLeanSun,
                        fowardLeanMon,
                        fowardLeanTue,
                        fowardLeanWed,
                        fowardLeanThu,
                        fowardLeanFri,
                        fowardLeanSat,
                    });
                    StatsViewModel.SetAxisLabels(new string[] {
                       // $"{analyticsPerWeek[sun].Date.Month}/{analyticsPerWeek[sun].Date.Day}",
                       // $"{analyticsPerWeek[mon].Date.Month}/{analyticsPerWeek[mon].Date.Day}",
                        //$"{analyticsPerWeek[tue].Date.Month}/{analyticsPerWeek[tue].Date.Day}",
                        //$"{analyticsPerWeek[wed].Date.Month}/{analyticsPerWeek[wed].Date.Day}",
                        //$"{analyticsPerWeek[thu].Date.Month}/{analyticsPerWeek[thu].Date.Day}",
                        //$"{analyticsPerWeek[fri].Date.Month}/{analyticsPerWeek[fri].Date.Day}",
                        //$"{analyticsPerWeek[sat].Date.Month}/{analyticsPerWeek[sat].Date.Day}",

                    });
                    myLabel.Text = "(時間)";
                    break;
                case 2:
                    //var analyticsPerMonth = StatisticsProvider.GetAnalyticsPerMonth(date);
                    //int weekCount = analyticsPerMonth.Count;
                    int weekCount = 5;
                    float[] monthActivateTimes = new float[weekCount];
                    float[] monthForwardLeanTimes = new float[weekCount];
                    string[] monthAxis = new string[weekCount];
                    for (int i = 0; i < weekCount; i++)
                    {
                        //monthActivateTimes[i] = analyticsPerMonth[i].ActiveTime;
                        monthActivateTimes[i] = i * 4;
                        //monthForwardLeanTimes[i] = analyticsPerMonth[i].ForwardLeanTime;
                        monthForwardLeanTimes[i] = i * 5 + 3;
                        monthAxis[i] = $"{date.Month}月第{i}週";
                    }

                    StatsViewModel.SetStartUpTime(monthActivateTimes);
                    StatsViewModel.SetPoorPostureTime(monthForwardLeanTimes);
                    StatsViewModel.SetAxisLabels(monthAxis);
                    myLabel.Text = "(時間)";
                    break;
            }

            StatsViewModel.Series = StatsViewModel.Series;
            StatsViewModel.XAxes = StatsViewModel.XAxes;
            StatsViewModel.UpdateGraph();
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Settings");
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedIndex = picker.SelectedIndex;
            Label myLabel = this.FindByName<Label>("unit");
            
            switch (selectedIndex)
            {
                case 0:
                    date = date.AddDays(-1);
                    //var analyticsPerDay = StatisticsProvider.GetAnalyticsPerDay(date);
                    float[] dayActivateTimes = new float[24];
                    float[] dayForwardLeanTimes = new float[24];
                    for (int i = 0; i < 24; i++)
                    {
                        //dayActivateTimes[i] = analyticsPerDay[i].ActiveTime;
                        dayActivateTimes[i] = i * 4;
                        //dayForwardLeanTimes[i] = analyticsPerDay[i].ForwardLeanTime;
                        dayForwardLeanTimes[i] = i * 6;
                    }

                    StatsViewModel.SetStartUpTime(dayActivateTimes);

                    StatsViewModel.SetPoorPostureTime(dayForwardLeanTimes);
                    StatsViewModel.SetAxisLabels(new string[] {
                        $"{date.Month}/{date.Day} 0時",
                        "1時",
                        "2時",
                        "3時",
                        "4時",
                        "5時",
                        "6時",
                        "7時",
                        "8時",
                        "9時",
                        "10時",
                        "11時",
                        "12時",
                        "13時",
                        "14時",
                        "15時",
                        "16時",
                        "17時",
                        "18時",
                        "19",
                        "20時",
                        "21時",
                        "22時",
                        "23時",
                    });
                    myLabel.Text = "(分)";
                    break;
                case 1:
                    date = date.AddDays(-7);
                    //var analyticsPerWeek = StatisticsProvider.GetAnalyticsPerWeek(date);
                    int sun = (int)DayOfWeek.SUN;
                    int mon = (int)DayOfWeek.MON;
                    int tue = (int)DayOfWeek.TUE;
                    int wed = (int)DayOfWeek.WED;
                    int thu = (int)DayOfWeek.THU;
                    int fri = (int)DayOfWeek.FRI;
                    int sat = (int)DayOfWeek.SAT;

                    //float ActivateSun = analyticsPerWeek[sun].ActiveTime;
                    //float ActivateMon = analyticsPerWeek[mon].ActiveTime;
                    //float ActivateTue = analyticsPerWeek[tue].ActiveTime;

                    //float ActivateWed = analyticsPerWeek[wed].ActiveTime;
                    //float ActivateThu = analyticsPerWeek[thu].ActiveTime;
                    //float ActivateFri = analyticsPerWeek[fri].ActiveTime;
                    //float ActivateSat = analyticsPerWeek[sat].ActiveTime;

                    //float fowardLeanSun = analyticsPerWeek[sun].ForwardLeanTime;
                    //float fowardLeanMon = analyticsPerWeek[mon].ForwardLeanTime;
                    //float fowardLeanTue = analyticsPerWeek[tue].ForwardLeanTime;
                    //float fowardLeanWed = analyticsPerWeek[wed].ForwardLeanTime;
                    //float fowardLeanThu = analyticsPerWeek[fri].ForwardLeanTime;
                    //float fowardLeanFri = analyticsPerWeek[sat].ForwardLeanTime;
                    //float fowardLeanSat = analyticsPerWeek[sun].ForwardLeanTime;


                    float ActivateSun = 4;
                    float ActivateMon = 23;
                    float ActivateTue = 15;

                    float ActivateWed = 46;
                    float ActivateThu = 73;
                    float ActivateFri = 11;
                    float ActivateSat = 23;

                    float fowardLeanSun = 44;
                    float fowardLeanMon = 55;
                    float fowardLeanTue = 62;
                    float fowardLeanWed = 13;
                    float fowardLeanThu = 13;
                    float fowardLeanFri = 42;
                    float fowardLeanSat = 6;

                    StatsViewModel.SetStartUpTime(new float[] {
                        ActivateSun,
                        ActivateMon,
                        ActivateTue,
                        ActivateWed,
                        ActivateThu,
                        ActivateFri,
                        ActivateSat,
                        });
                    StatsViewModel.SetPoorPostureTime(new float[] {
                        fowardLeanSun,
                        fowardLeanMon,
                        fowardLeanTue,
                        fowardLeanWed,
                        fowardLeanThu,
                        fowardLeanFri,
                        fowardLeanSat,
                    });
                    StatsViewModel.SetAxisLabels(new string[] {
                       // $"{analyticsPerWeek[sun].Date.Month}/{analyticsPerWeek[sun].Date.Day}",
                       // $"{analyticsPerWeek[mon].Date.Month}/{analyticsPerWeek[mon].Date.Day}",
                        //$"{analyticsPerWeek[tue].Date.Month}/{analyticsPerWeek[tue].Date.Day}",
                        //$"{analyticsPerWeek[wed].Date.Month}/{analyticsPerWeek[wed].Date.Day}",
                       // $"{analyticsPerWeek[thu].Date.Month}/{analyticsPerWeek[thu].Date.Day}",
                       // $"{analyticsPerWeek[fri].Date.Month}/{analyticsPerWeek[fri].Date.Day}",
                       // $"{analyticsPerWeek[sat].Date.Month}/{analyticsPerWeek[sat].Date.Day}",

                    });
                    myLabel.Text = "(時間)";
                    break;
                case 2:
                    date = date.AddMonths(-1);
                    //var analyticsPerMonth = StatisticsProvider.GetAnalyticsPerMonth(date);
                    //int weekCount = analyticsPerMonth.Count;
                    int weekCount = 5;
                    float[] monthActivateTimes = new float[weekCount];
                    float[] monthForwardLeanTimes = new float[weekCount];
                    string[] monthAxis = new string[weekCount];
                    for (int i = 0; i < weekCount; i++)
                    {
                        //monthActivateTimes[i] = analyticsPerMonth[i].ActiveTime;
                        monthActivateTimes[i] = i * 4 + 3;
                        //monthForwardLeanTimes[i] = analyticsPerMonth[i].ForwardLeanTime;
                        monthForwardLeanTimes[i] = i * 5 + 2;
                        monthAxis[i] = $"{date.Month}月第{i}週";
                    }

                    StatsViewModel.SetStartUpTime(monthActivateTimes);
                    StatsViewModel.SetPoorPostureTime(monthForwardLeanTimes);
                    StatsViewModel.SetAxisLabels(monthAxis);
                    myLabel.Text = "(時間)";
                    break;
            }

            StatsViewModel.Series = StatsViewModel.Series;
            StatsViewModel.XAxes = StatsViewModel.XAxes;
            StatsViewModel.UpdateGraph();
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedIndex = picker.SelectedIndex;
            Label myLabel = this.FindByName<Label>("unit");

            switch (selectedIndex)
            {
                case 0:
                    date = date.AddDays(1);
                    //var analyticsPerDay = StatisticsProvider.GetAnalyticsPerDay(date);
                    float[] dayActivateTimes = new float[24];
                    float[] dayForwardLeanTimes = new float[24];
                    for (int i = 0; i < 24; i++)
                    {
                        //dayActivateTimes[i] = analyticsPerDay[i].ActiveTime;
                        dayActivateTimes[i] = i * 4 + 2;
                        //dayForwardLeanTimes[i] = analyticsPerDay[i].ForwardLeanTime;
                        dayForwardLeanTimes[i] = i * 2 + 12;
                    }

                    StatsViewModel.SetStartUpTime(dayActivateTimes);

                    StatsViewModel.SetPoorPostureTime(dayForwardLeanTimes);
                    StatsViewModel.SetAxisLabels(new string[] {
                        $"{date.Month}/{date.Day} 0時",
                        "1時",
                        "2時",
                        "3時",
                        "4時",
                        "5時",
                        "6時",
                        "7時",
                        "8時",
                        "9時",
                        "10時",
                        "11時",
                        "12時",
                        "13時",
                        "14時",
                        "15時",
                        "16時",
                        "17時",
                        "18時",
                        "19時",
                        "20時",
                        "21時",
                        "22時",
                        "23時",
                    });
                    myLabel.Text = "(分)";
                    break;
                case 1:
                    date = date.AddDays(7);
                    //var analyticsPerWeek = StatisticsProvider.GetAnalyticsPerWeek(date);
                    int sun = (int)DayOfWeek.SUN;
                    int mon = (int)DayOfWeek.MON;
                    int tue = (int)DayOfWeek.TUE;
                    int wed = (int)DayOfWeek.WED;
                    int thu = (int)DayOfWeek.THU;
                    int fri = (int)DayOfWeek.FRI;
                    int sat = (int)DayOfWeek.SAT;

                    

                    //float ActivateSun = analyticsPerWeek[sun].ActiveTime;
                    //float ActivateMon = analyticsPerWeek[mon].ActiveTime;
                    //float ActivateTue = analyticsPerWeek[tue].ActiveTime;

                    //float ActivateWed = analyticsPerWeek[wed].ActiveTime;
                    //float ActivateThu = analyticsPerWeek[thu].ActiveTime;
                    //float ActivateFri = analyticsPerWeek[fri].ActiveTime;
                    //float ActivateSat = analyticsPerWeek[sat].ActiveTime;

                    //float fowardLeanSun = analyticsPerWeek[sun].ForwardLeanTime;
                    //float fowardLeanMon = analyticsPerWeek[mon].ForwardLeanTime;
                    //float fowardLeanTue = analyticsPerWeek[tue].ForwardLeanTime;
                    //float fowardLeanWed = analyticsPerWeek[wed].ForwardLeanTime;
                    //float fowardLeanThu = analyticsPerWeek[fri].ForwardLeanTime;
                    //float fowardLeanFri = analyticsPerWeek[sat].ForwardLeanTime;
                    //float fowardLeanSat = analyticsPerWeek[sun].ForwardLeanTime;


                    float ActivateSun = 4;
                    float ActivateMon = 23;
                    float ActivateTue = 15;

                    float ActivateWed = 46;
                    float ActivateThu = 73;
                    float ActivateFri = 11;
                    float ActivateSat = 23;

                    float fowardLeanSun = 44;
                    float fowardLeanMon = 55;
                    float fowardLeanTue = 62;
                    float fowardLeanWed = 13;
                    float fowardLeanThu = 13;
                    float fowardLeanFri = 42;
                    float fowardLeanSat = 6;

                    StatsViewModel.SetStartUpTime(new float[] {
                        ActivateSun,
                        ActivateMon,
                        ActivateTue,
                        ActivateWed,
                        ActivateThu,
                        ActivateFri,
                        ActivateSat,
                        });
                    StatsViewModel.SetPoorPostureTime(new float[] {
                        fowardLeanSun,
                        fowardLeanMon,
                        fowardLeanTue,
                        fowardLeanWed,
                        fowardLeanThu,
                        fowardLeanFri,
                        fowardLeanSat,
                    });
                    StatsViewModel.SetAxisLabels(new string[] {
                        //$"{analyticsPerWeek[sun].Date.Month}/{analyticsPerWeek[sun].Date.Day}",
                       // $"{analyticsPerWeek[mon].Date.Month}/{analyticsPerWeek[mon].Date.Day}",
                       // $"{analyticsPerWeek[tue].Date.Month}/{analyticsPerWeek[tue].Date.Day}",
                       // $"{analyticsPerWeek[wed].Date.Month}/{analyticsPerWeek[wed].Date.Day}",
                       // $"{analyticsPerWeek[thu].Date.Month}/{analyticsPerWeek[thu].Date.Day}",
                        //$"{analyticsPerWeek[fri].Date.Month}/{analyticsPerWeek[fri].Date.Day}",
                        //$"{analyticsPerWeek[sat].Date.Month}/{analyticsPerWeek[sat].Date.Day}",

                    });
                    myLabel.Text = "(時間)";
                    break;
                case 2:
                    date = date.AddMonths(1);
                    //var analyticsPerMonth = StatisticsProvider.GetAnalyticsPerMonth(date);
                    //int weekCount = analyticsPerMonth.Count;
                    int weekCount = 4;
                    float[] monthActivateTimes = new float[weekCount];
                    float[] monthForwardLeanTimes = new float[weekCount];
                    string[] monthAxis = new string[weekCount];
                    for (int i = 0; i < weekCount; i++)
                    {
                        //monthActivateTimes[i] = analyticsPerMonth[i].ActiveTime;
                        monthActivateTimes[i] = i * 6;
                        //monthForwardLeanTimes[i] = analyticsPerMonth[i].ForwardLeanTime;
                        monthForwardLeanTimes[i] = i * 9;
                        monthAxis[i] = $"{date.Month}月第{i}週";
                    }

                    StatsViewModel.SetStartUpTime(monthActivateTimes);
                    StatsViewModel.SetPoorPostureTime(monthForwardLeanTimes);
                    StatsViewModel.SetAxisLabels(monthAxis);
                    myLabel.Text = "(時間)";
                    break;
            }

            StatsViewModel.Series = StatsViewModel.Series;
            StatsViewModel.XAxes = StatsViewModel.XAxes;
            StatsViewModel.UpdateGraph();
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