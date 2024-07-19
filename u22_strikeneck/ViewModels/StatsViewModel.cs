using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Maui.Graphics;

namespace u22_strikeneck.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        private ISeries[] series;
        private static List<float> startUpTime = new List<float>();
        private static List<float> poorPostureTime = new List<float>();
        private static List<string> axisLabels = new List<string>();

        public StatsViewModel()
        {
            series = new ISeries[]
            {
                new ColumnSeries<float> {
                    Values = startUpTime.ToArray(),
                    MaxBarWidth = 60,
                    IgnoresBarPosition = true
                },
                new ColumnSeries<float> {
                    Values = poorPostureTime.ToArray(),
                    MaxBarWidth = 30,
                    IgnoresBarPosition = true
                }
            };

            xAxes = new[]
            {
                new Axis { Labels = axisLabels.ToArray() }
            };
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

        private Axis[] xAxes;

        public Axis[] XAxes
        {
            get { return xAxes; }
            set
            {
                xAxes = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void SetStartUpTime(List<float> newValues)
        {
            startUpTime = newValues;
        }

        public static void SetPoorPostureTime(List<float> newValues)
        {
            poorPostureTime = newValues;
        }

        public void UpdateGraph()
        {
            series = new ISeries[]
            {
                new ColumnSeries<float> {
                    Values = startUpTime.ToArray(),
                    Stroke = null,
                    MaxBarWidth = 60,
                    IgnoresBarPosition = true,
                },
                new ColumnSeries<float> {
                    Values = poorPostureTime.ToArray(),
                    Stroke = null,
                    MaxBarWidth = 30,
                    IgnoresBarPosition = true,
                }
            };

            XAxes = new[]
            {
                new Axis { Labels = axisLabels.ToArray() }
            };

            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(XAxes));
        }

        public static void SetAxisLabels(List<string> newValues)
        {
            axisLabels = newValues;
        }
    }
}