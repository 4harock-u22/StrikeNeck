using System;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace u22_strikeneck
{
    public class PostureEvent
    {
        [PrimaryKey]
        public DateTime Timestamp { get; set; }
        public int Check { get; set; }
        public int Detection { get; set; }
    }

    public class DatabaseWriter
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseWriter()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "posture_data.db");
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeDatabase().Wait();
        }

        private async Task InitializeDatabase()
        {
            await _database.CreateTableAsync<PostureEvent>();
        }

        public async Task UpdateOrInsertPostureEventAsync(DateTime timestamp, bool isDistortionDetected)
        {
            var existingEvent = await _database.Table<PostureEvent>()
                                               .Where(x => x.Timestamp == timestamp)
                                               .FirstOrDefaultAsync();
            if (existingEvent != null)
            {
                existingEvent.Check += 1;
                if (isDistortionDetected)
                {
                    existingEvent.Detection += 1;
                }
                await _database.UpdateAsync(existingEvent);
            }
            else
            {
                var newPostureEvent = new PostureEvent
                {
                    Timestamp = timestamp,
                    Check = 1,
                    Detection = isDistortionDetected ? 1 : 0
                };
                await _database.InsertAsync(newPostureEvent);
            }
        }
    }
}