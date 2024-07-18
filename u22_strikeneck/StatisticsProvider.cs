using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
            List<AnalyticsData> analyticsDataList = new List<AnalyticsData>(24);
            for (int i = 0; i < 24; i++)
            {
                if (postureEventsByHour[i].Timestamp.Hour == i)
                {
                    analyticsDataList[i] = new AnalyticsData
                    {
                        ActiveTime = postureEventsByHour[i].Check * checkPostureInterval,
                        ForwardLeanTime = postureEventsByHour[i].Detection * checkPostureInterval,
                        Date = new DateTime(date.Year, date.Month, date.Day, i, 0, 0)
                    };
                }
                else
                {
                    analyticsDataList[i] = null;
                }
            }
            return analyticsDataList;
        }

        public static async Task<List<AnalyticsData>> GetAnalyticsPerWeekAsync(DateTime date)
        {
            DatabaseReader reader = new DatabaseReader();
            DateTime end = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(1);
            DateTime begin = end.AddDays(-7);
            var postureEventsByDay = await reader.GetAveragePostureEventsByDayAsync(begin, end);
            List<AnalyticsData> analyticsDataList = new List<AnalyticsData>(7);
            for (int i = 0; i < 7; i++)
            {
                if (postureEventsByDay[i].Timestamp.Day == begin.Day + i)
                {
                    analyticsDataList[i] = new AnalyticsData
                    {
                        ActiveTime = postureEventsByDay[i].Check * checkPostureInterval,
                        ForwardLeanTime = postureEventsByDay[i].Detection * checkPostureInterval,
                        Date = new DateTime(date.Year, date.Month, date.Day, i, 0, 0)
                    };
                }
                else
                {
                    analyticsDataList[i] = null;
                }
            }
            return analyticsDataList;
        }

        public static async Task<List<AnalyticsData>> GetAnalyticsPerMonthAsync(DateTime date)
        {
            DatabaseReader reader = new DatabaseReader();
            DateTime end = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(1);
            DateTime begin = end.AddDays(-30);
            var postureEventsByDay = await reader.GetAveragePostureEventsByDayAsync(begin, end);
            List<AnalyticsData> analyticsDataList = new List<AnalyticsData>(30);
            for (int i = 0; i < 30; i++)
            {
                if (postureEventsByDay[i].Timestamp.Day == begin.Day + i)
                {
                    analyticsDataList[i] = new AnalyticsData
                    {
                        ActiveTime = postureEventsByDay[i].Check * checkPostureInterval,
                        ForwardLeanTime = postureEventsByDay[i].Detection * checkPostureInterval,
                        Date = new DateTime(date.Year, date.Month, date.Day, i, 0, 0)
                    };
                }
                else
                {
                    analyticsDataList[i] = null;
                }
            }
            return analyticsDataList;
        }
    }
}