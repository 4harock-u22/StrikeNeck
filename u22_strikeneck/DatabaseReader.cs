using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SQLite;

namespace u22_strikeneck
{
    public class DatabaseReader
    {
        private SQLiteAsyncConnection _database;

        public DatabaseReader()
        {
        }

        private async Task Init()
        {
            if (_database is not null)
            {
                return;
            }
            string dbPath = Path.Combine(FileSystem.Current.AppDataDirectory, "posture_data.db");
            _database = new SQLiteAsyncConnection(dbPath, false);
            var result = await _database.CreateTableAsync<PostureEvent>();
        }

        public async Task<List<PostureEvent>> GetPostureEventsAsync(DateTime begin, DateTime end)
        {
            await Init();
            string beginDate = begin.ToString("yyyy-MM-dd HH:00:00");
            string endDate = end.ToString("yyyy-MM-dd HH:00:00");

            var query = "SELECT * FROM PostureEvent WHERE Timestamp >= ? AND Timestamp < ?";
            var result = await _database.QueryAsync<PostureEvent>(query, beginDate, endDate);
            if (result is not null)
            {
                return result;
            }
            else
            {
                return new List<PostureEvent>();
            }
        }

        public async Task<List<PostureEvent>> GetAveragePostureEventsByDayAsync(DateTime begin, DateTime end)
        {
            await Init();
            var postureEvents = await GetPostureEventsAsync(begin, end);
            if (postureEvents.Count == 0)
            {
                return new List<PostureEvent>();
            }

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