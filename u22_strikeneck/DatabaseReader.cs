using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<List<PostureEvent>> GetPostureEventByTimestampAsync(DateTime timestamp)
        {
            // 年, 月, 日, 時間 のみをのこし不要な 分, 秒 は削除
            DateTime trimmedTimestamp = new DateTime(
                timestamp.Year,
                timestamp.Month,
                timestamp.Day,
                timestamp.Hour,
                0,
                0
            );
            return await _database.Table<PostureEvent>()
                                  .Where(x => x.Timestamp == trimmedTimestamp)
                                  .ToListAsync();
        }
    }
}