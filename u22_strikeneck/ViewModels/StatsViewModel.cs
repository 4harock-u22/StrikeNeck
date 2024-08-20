using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Maui.Graphics;
using Microsoft.VisualBasic;
using SkiaSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace u22_strikeneck.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<float> startUpTime = new ObservableCollection<float>();
        private ObservableCollection<float> poorPostureTime = new ObservableCollection<float>();
        private ObservableCollection<string> axisLabels = new ObservableCollection<string>();

        private ISeries[] series;
        private Axis[] xAxes;
        private Axis[] yAxes;
        private DateTime currentDate;
        private int selectedIndex;

        public StatsViewModel()
        {
            currentDate = DateTime.Now;
            UpdateGraph();
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetStartUpTime(List<float> newValues)
        {
            for (int i = newValues.Count - 1; i >= 0; i--)
            {
                startUpTime.Insert(0, newValues[i]);
            }
        }

        public void SetPoorPostureTime(List<float> newValues)
        {
            for (int i = newValues.Count - 1; i >= 0; i--)
            {
                poorPostureTime.Insert(0, newValues[i]);
            }
        }

        public void SetAxisLabels(List<string> newValues)
        {
            for (int i = newValues.Count - 1; i >= 0; i--)
            {
                axisLabels.Insert(0, newValues[i]);
            }
        }

        public void UpdateGraph()
        {
            Series = new ISeries[]
            {
                new ColumnSeries<float> {
                    Values = startUpTime.ToArray(),
                    Stroke = null,
                    MaxBarWidth = 48,
                    IgnoresBarPosition = true,
                    Fill = new SolidColorPaint(SKColor.Parse("#26595A"))
                },
                new ColumnSeries<float> {
                    Values = poorPostureTime.ToArray(),
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
                    Labels = axisLabels.ToArray() 
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

        public async Task SetDataAsync()
        {
            var analytics = await StatisticsProvider.GetAnalytics(selectedIndex, currentDate);

            SetStartUpTime(analytics.ActivateTimes);
            SetPoorPostureTime(analytics.ForwardLeanTimes);
            SetAxisLabels(analytics.AxisLabels);

            UpdateGraph();
        }

        public async Task LoadMoreDataAsync()
        {
            AdjustDateBackward();
            await SetDataAsync();
        }

        public void SetSelectedIndex(int newIndex)
        {
            selectedIndex = newIndex;
            UpdateGraph();
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
    }
}