using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using SQLite;

namespace u22_strikeneck
{
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

        public async Task<List<PostureEvent>> GetPostureEventsAsync(DateTime begin, DateTime end)
        {
            // DateTimeを文字列に変換
            string beginDate = begin.ToString("yyyy-MM-dd HH:00:00");
            string endDate = end.ToString("yyyy-MM-dd HH:00:00");

            var query = "SELECT * FROM PostureEvent WHERE Timestamp >= ? AND Timestamp < ?";
            return await _database.QueryAsync<PostureEvent>(query, beginDate, endDate);
        }

        public async Task<List<PostureEvent>> GetAveragePostureEventsByDayAsync(DateTime begin, DateTime end)
        {
            var postureEvents = await GetPostureEventsAsync(begin, end);

            var groupedEvents = postureEvents.GroupBy(e => e.Timestamp.Date);

            var averageEvents = groupedEvents.Select(g => new PostureEvent
            {
                Timestamp = g.Min(e => e.Timestamp),
                Check = g.Average(e => e.Check),
                Detection = g.Average(e => e.Detection)
            }).ToList();

            return averageEvents;
        }
    }
}