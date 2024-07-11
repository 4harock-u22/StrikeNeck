using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace u22_strikeneck
{
    public class PostureEvent
    {
        [PrimaryKey, AutoIncrement]
        public string Timestamp { get; set; }
        public bool isBad { get; set; }
    }

    public class PostureDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public PostureDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeDatabase().Wait();
        }

        private async Task InitializeDatabase()
        {
            await _database.CreateTableAsync<PostureEvent>();
        }

        public async Task SavePostureEventAsync()
        {
            var postureEvent = new PostureEvent
            {
                Timestamp = DateTime.Now.ToString("o")
            };
            await _database.InsertAsync(postureEvent);
        }
    }
}