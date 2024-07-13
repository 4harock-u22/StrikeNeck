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
            DatabaseReader reader = new DatabaseReader();
            var eventPosture = await reader.GetPostureEventByTimestampAsync(date);
            float activeTime = eventPosture.Check * 60;
            float forwardLeanTime = eventPosture.Detection * 60;
            return new List<AnalyticsData>
            {
                new AnalyticsData
                {
                    ActiveTime = activeTime,
                    ForwardLeanTime = forwardLeanTime,
                    Date = date,
                }
            };
        }

        public static async Task<List<AnalyticsData>> GetAnalyticsPerWeekAsync(DateTime date)
        {

        }

        public static async Task<List<AnalyticsData>> GetAnalyticsPerMonthAsync(DateTime date)
        {

        }
    }
}