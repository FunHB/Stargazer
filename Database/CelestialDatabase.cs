using SQLite;
using Stargazer.Database.Models;
using System;
using System.Linq.Expressions;

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
            var result = await database.CreateTableAsync<Star>();
            var resultPlanet = await database.CreateTableAsync<Planet>();
        }

        public async Task<List<T>> GetItemsAsync<T>() where T : ICelestialBody, new()
        {
            await Init();
            return await database.Table<T>().ToListAsync();
        }

        public async Task<T?> GetItemAsync<T>(int id) where T : ICelestialBody, new()
        {
            await Init();
            return await database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T?> GetItemAsync<T>(Expression<Func<T, bool>> predicate) where T : ICelestialBody, new()
        {
            await Init();
            return await database.Table<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync<T>(T item) where T : ICelestialBody, new()
        {
            await Init();
            if (item.Id != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync<T>(int id) where T : ICelestialBody, new()
        {
            await Init();
            return await database.DeleteAsync(id);
        }

        public async Task<int> DeleteItemAsync<T>(T item) where T : ICelestialBody, new()
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}
