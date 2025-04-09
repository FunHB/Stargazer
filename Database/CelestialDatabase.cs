using SQLite;
using Stargazer.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stargazer.Database
{
    public class CelestialDatabase
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<Entity>();
        }

        public async Task<List<T>> GetItemsAsync<T>() where T : Entity, new()
        {
            await Init();
            return await database.Table<T>().ToListAsync();
        }

        public async Task<T> GetItemAsync<T>(int id) where T : Entity, new()
        {
            await Init();
            return await database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> FindItemAsync<T>(Func<T, bool> query) where T : Entity, new()
        {
            await Init();
            return await database.Table<T>().Where(i => query(i)).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync<T>(T item) where T : Entity, new()
        {
            await Init();
            if (item.Id != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync<T>(T item) where T : Entity, new()
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
