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
        public static async Task<List<AnalyticsData>> GetAnalyticsPerDayAsync(DateTime date)
        {
            // 年, 月, 日 のみをのこす
            DateTime trimmedDate = new DateTime(
                date.Year,
                date.Month,
                date.Day,
                0,
                0,
                0
            );
            DatabaseReader reader = new DatabaseReader();
            var datasOfDay = await reader.GetPostureEventByTimestampAsync(trimmedDate);
            List<AnalyticsData> analyticsDataList = new List<AnalyticsData>(24);
            for ( int i = 0; i < 24;  i++ )
            {
                if (datasOfDay[i].Timestamp.Hour == i)
                {
                    analyticsDataList[i] = new AnalyticsData
                    {
                        ActiveTime = datasOfDay[i].Check * 1,
                        ForwardLeanTime = datasOfDay[i].Detection * 1,
                        Date = new DateTime(
                            trimmedDate.Year,
                            trimmedDate.Month,
                            trimmedDate.Day,
                            i,
                            0,
                            0
                        )
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

        }

        public static async Task<List<AnalyticsData>> GetAnalyticsPerMonthAsync(DateTime date)
        {

        }
    }
}