using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Maui.Graphics;
using SkiaSharp;

namespace u22_strikeneck.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        private List<float> startUpTime = new List<float>();
        private List<float> poorPostureTime = new List<float>();
        private List<string> axisLabels = new List<string>();

        private ISeries[] series;
        private Axis[] xAxes;
        private Axis[] yAxes;

        public StatsViewModel()
        {
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
            startUpTime = newValues;
        }

        public void SetPoorPostureTime(List<float> newValues)
        {
            poorPostureTime = newValues;
        }

        public void SetAxisLabels(List<string> newValues)
        {
            axisLabels = newValues;
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
    }
}