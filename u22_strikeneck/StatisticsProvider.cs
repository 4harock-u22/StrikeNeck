using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace u22_strikeneck
{
    public class AnalyticsData
    {
        public float ActiveTime { get; set; }
        public float ForwardLeanTime { get; set; }
        public DateTime Date { get; set; } // 週次や月次の場合の日付
    }

    public class StatsData
    {
        public List<float> ActivateTimes { get; set; }
        public List<float> ForwardLeanTimes { get; set; }
        public List<string> AxisLabels { get; set; }
    }

    public static class StatisticsProvider
    {
        private const int checkPostureInterval = 1;
        public static async Task<StatsData> GetAnalytics(int selectedIndex, DateTime date)
        {
            switch (selectedIndex)
            {
                case 0:
                    return await ProcessDailyAnalyticsData(date, 24);
                case 1:
                    return await ProcessWeeklyAnalyticsData(date, 7);
                case 2:
                    return await ProcessMonthlyAnalyticsData(date, 30);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<StatsData> ProcessDailyAnalyticsData(DateTime date, int length)
        {
            var analyticsData = await GetAnalyticsPerDayAsync(date);
            var result = new StatsData
            {
                ActivateTimes = analyticsData.Take(length).Select(apd => apd.ActiveTime).ToList(),
                ForwardLeanTimes = analyticsData.Take(length).Select(apd => apd.ForwardLeanTime).ToList(),
                AxisLabels = analyticsData.Take(length)
                                          .Select(apd => $"{apd.Date.Hour}:00")
                                          .ToList()
            };
            result.AxisLabels[0] = date.ToString("M/d 00:00");
            return result;
        }

        private static async Task<StatsData> ProcessWeeklyAnalyticsData(DateTime date, int length)
        {
            var analyticsData = await GetAnalyticsPerWeekAsync(date);
            var result = new StatsData
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

        private static async Task<StatsData> ProcessMonthlyAnalyticsData(DateTime date, int length)
        {
            var analyticsData = await GetAnalyticsPerMonthAsync(date);
            var result = new StatsData
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

        private static async Task<List<AnalyticsData>> GetAnalyticsPerDayAsync(DateTime date)
        {
            DatabaseReader reader = new DatabaseReader();

            DateTime end = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(1);
            DateTime begin = end.AddDays(-1);
            var postureEventsByHour = await reader.GetPostureEventsAsync(begin, end);
            List<AnalyticsData> analyticsDataList = new List<AnalyticsData>();
            int index = 0;
            for (int i = 0; i < 24; i++)
            {
                var dateTime = new DateTime(date.Year, date.Month, date.Day, i, 0, 0);
                var data = new AnalyticsData
                {
                    ActiveTime = 0,
                    ForwardLeanTime = 0,
                    Date = dateTime
                };

                if (postureEventsByHour.Count > 0 && index < postureEventsByHour.Count)
                {
                    if (postureEventsByHour[index].Timestamp.Hour == i)
                    {
                        data.ActiveTime = postureEventsByHour[index].Check * checkPostureInterval;
                        data.ForwardLeanTime = postureEventsByHour[index].Detection * checkPostureInterval;
                        index++;
                    }
                }

                analyticsDataList.Add(data);
            }
            return analyticsDataList;
        }

        private static async Task<List<AnalyticsData>> GetAnalyticsPerWeekAsync(DateTime date)
        {
            DatabaseReader reader = new DatabaseReader();

            DateTime end = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(1);
            DateTime begin = end.AddDays(-7);
            var postureEventsByDay = await reader.GetAveragePostureEventsByDayAsync(begin, end);
            List<AnalyticsData> analyticsDataList = new List<AnalyticsData>();
            int index = 0;
            for (int i = 0; i < 7; i++)
            {
                var dateTime = new DateTime(begin.Year, begin.Month, begin.Day, 0, 0, 0).AddDays(i);
                var data = new AnalyticsData
                {
                    ActiveTime = 0,
                    ForwardLeanTime = 0,
                    Date = dateTime
                };

                if (postureEventsByDay.Count > 0 && index < postureEventsByDay.Count)
                {
                    if (postureEventsByDay[index].Timestamp == data.Date)
                    {
                        data.ActiveTime = postureEventsByDay[index].Check * checkPostureInterval;
                        data.ForwardLeanTime = postureEventsByDay[index].Detection * checkPostureInterval;
                        index++;
                    }
                }

                analyticsDataList.Add(data);
            }
            return analyticsDataList;
        }

        private static async Task<List<AnalyticsData>> GetAnalyticsPerMonthAsync(DateTime date)
        {
            DatabaseReader reader = new DatabaseReader();

            DateTime end = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(1);
            DateTime begin = end.AddDays(-30);
            var postureEventsByDay = await reader.GetAveragePostureEventsByDayAsync(begin, end);
            List<AnalyticsData> analyticsDataList = new List<AnalyticsData>();
            int index = 0;
            for (int i = 0; i < 30; i++)
            {
                var dateTime = new DateTime(begin.Year, begin.Month, begin.Day, 0, 0, 0).AddDays(i);
                var data = new AnalyticsData
                {
                    ActiveTime = 0,
                    ForwardLeanTime = 0,
                    Date = dateTime
                };

                if (postureEventsByDay.Count > 0 && index < postureEventsByDay.Count)
                {
                    if (postureEventsByDay[index].Timestamp == data.Date)
                    {
                        data.ActiveTime = postureEventsByDay[index].Check * checkPostureInterval;
                        data.ForwardLeanTime = postureEventsByDay[index].Detection * checkPostureInterval;
                        index++;
                    }
                }

                analyticsDataList.Add(data);
            }
            return analyticsDataList;
        }
    }
}