using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Maui.Graphics;
using SkiaSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace u22_strikeneck.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        private StatsData StatsDatas = new StatsData();

        private ISeries[] series;
        private Axis[] xAxes;
        private Axis[] yAxes;
        private int selectedIndex;
        private DateTime currentDate;

        public StatsViewModel()
        {
            currentDate = DateTime.Now;
            UpdateAnalytics();
        }

        public ISeries[] Series
        {
            get { return series; }
            set
            {
                series = value;
                OnPropertyChanged();
            }
        }

        public Axis[] XAxes
        {
            get { return xAxes; }
            set
            {
                xAxes = value;
                OnPropertyChanged();
            }
        }

        public Axis[] YAxes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task UpdateStatsAsync(bool isBack = false, bool isNext = false)
        {
            if (isBack)
            {
                AdjustDateBackward();
            }
            else if (isNext)
            {
                AdjustDateNextward();
            }

            var analytics = await StatisticsProvider.GetAnalytics(selectedIndex, currentDate);

            SetStartUpTime(analytics.ActivateTimes);
            SetPoorPostureTime(analytics.ForwardLeanTimes);
            SetAxisLabels(analytics.AxisLabels);

            UpdateAnalytics();
        }

        private void AdjustDateBackward()
        {
            switch (selectedIndex)
            {
                case 0: currentDate = currentDate.AddDays(-1); break;
                case 1: currentDate = currentDate.AddDays(-7); break;
                case 2: currentDate = currentDate.AddMonths(-1); break;
            }
        }

        private void AdjustDateNextward()
        {
            switch (selectedIndex)
            {
                case 0: currentDate = currentDate.AddDays(1); break;
                case 1: currentDate = currentDate.AddDays(7); break;
                case 2: currentDate = currentDate.AddMonths(1); break;
            }
        }

        public void SetSelectedIndex(int newIndex)
        {
            selectedIndex = newIndex;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetStartUpTime(List<float> newValues)
        {
            StatsDatas.ActivateTimes = newValues;
        }

        public void SetPoorPostureTime(List<float> newValues)
        {
            StatsDatas.ForwardLeanTimes = newValues;
        }

        public void SetAxisLabels(List<string> newValues)
        {
            StatsDatas.AxisLabels = newValues;
        }

        public void UpdateAnalytics()
        {
            Series = new ISeries[]
            {
                new ColumnSeries<float> {
                    Values = StatsDatas.ActivateTimes.ToArray(),
                    Stroke = null,
                    MaxBarWidth = 48,
                    IgnoresBarPosition = true,
                    Fill = new SolidColorPaint(SKColor.Parse("#26595A"))
                },
                new ColumnSeries<float> {
                    Values = StatsDatas.ForwardLeanTimes.ToArray(),
                    Stroke = null,
                    MaxBarWidth = 24,
                    IgnoresBarPosition = true,
                    Fill = new SolidColorPaint(SKColor.Parse("#87E3E0"))
                }
            };

            XAxes = new[]
            {
                new Axis 
                { 
                    Labels = StatsDatas.AxisLabels.ToArray() 
                }
            };

            YAxes = new[]
            {
                new Axis
                {
                    MaxLimit = 60,
                    MinLimit = 0
                }
            };
        }
    }
}