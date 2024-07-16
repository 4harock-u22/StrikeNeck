using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace u22_strikeneck
{
    public enum GroupKey
    {
        Day,
        Week
    }

    public class DatabaseReader
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseReader()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "posture_data.db");
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeDatabase().Wait();
        }

        private async Task InitializeDatabase()
        {
            await _database.CreateTableAsync<PostureEvent>();
        }

        public async Task<List<PostureEvent>> GetPostureEventByTimestampAsync(DateTime timestamp)
        {
            string dateString = timestamp.ToString("yyyy-MM-dd");

            return await _database.Table<PostureEvent>()
                                  .Where(x => x.Timestamp.ToString("yyyy-MM-dd").StartsWith(dateString))
                                  .ToListAsync();
        }

        public async Task<List<PostureEvent>> GetGroupedPostureEventsAsync(DateTime begin, DateTime end, GroupKey key)
        {
            var postureEvents = await _database.Table<PostureEvent>()
                                               .Where(e => e.Timestamp >= begin && e.Timestamp <= end)
                                               .ToListAsync();

            IEnumerable<IGrouping<object, PostureEvent>> groupedEvents = key switch
            {
                GroupKey.Day => postureEvents.GroupBy(e => (object)e.Timestamp.Date),
                GroupKey.Week => postureEvents.GroupBy(e => (object)CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(e.Timestamp, CalendarWeekRule.FirstDay, DayOfWeek.Sunday)),
                _ => throw new ArgumentException("Invalid grouping key")
            };

            var averageEvents = groupedEvents.Select(g => new PostureEvent
            {
                Timestamp = key == GroupKey.Day ? (DateTime)g.Key : g.Min(e => e.Timestamp),
                Check = g.Average(e => e.Check),
                Detection = g.Average(e => e.Detection)
            }).ToList();

            return averageEvents;
        }
    }
}