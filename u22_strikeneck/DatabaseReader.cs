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

        public async Task<PostureEvent> GetPostureEventByTimestampAsync(DateTime timestamp)
        {
            return await _database.Table<PostureEvent>()
                                  .Where(x => x.Timestamp == timestamp)
                                  .FirstOrDefaultAsync();
        }
    }
}