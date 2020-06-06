using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite;
using CrimeWatch.Models;

namespace CrimeWatch
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Incident>().Wait();

        }

        public Task<List<Incident>> GetIncidentsAsync()
        {
            return _database.Table<Incident>().ToListAsync();
        }

        public void RemoveIncidentAsync(Incident incident)
        {
            _database.Table<Incident>().DeleteAsync(i => i.Description.Equals(incident.Description));
        }

        public void ClearIncidents()
        {
            _database.ExecuteAsync("DELETE FROM Incident");
        }

        public bool TableIsEmpty()
        {
            if (_database.Table<Incident>().FirstOrDefaultAsync().Result == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<int> SaveIncidentAsync(Incident i)
        {
            return _database.InsertAsync(i);
        }

    }
}
