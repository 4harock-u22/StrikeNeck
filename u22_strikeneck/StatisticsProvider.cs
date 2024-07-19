using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck
{
    public class AnalyticsData
    {
        public float ActiveTime { get; set; }
        public float ForwardLeanTime { get; set; }
        public DateTime Date { get; set; } // 週次や月次の場合の日付
    }

    public static class StatisticsProvider
    {
        // 姿勢検知の実行間隔(固定するので削除予定)
        private const int checkPostureInterval = 1;
        public static async Task<List<AnalyticsData>> GetAnalyticsPerDayAsync(DateTime date)
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

        public static async Task<List<AnalyticsData>> GetAnalyticsPerWeekAsync(DateTime date)
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
                    if (postureEventsByDay[index].Timestamp.Day == begin.Day + i)
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

        public static async Task<List<AnalyticsData>> GetAnalyticsPerMonthAsync(DateTime date)
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
                    if (postureEventsByDay[index].Timestamp.Day + (postureEventsByDay[index].Timestamp.Month - begin.Month) * DateTime.DaysInMonth(begin.Year, begin.Month) == begin.Day + i)
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