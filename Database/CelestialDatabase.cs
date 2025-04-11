using SQLite;
using Stargazer.Database.Models;
using System;
using System.Diagnostics;
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

        public async Task<int> CountItemsAsync<T>() where T : ICelestialBody, new() => await CountItemsAsync<T>(t => true);
        public async Task<int> CountItemsAsync<T>(Expression<Func<T, bool>> predicate) where T : ICelestialBody, new()
        {
            await Init();
            return await database.Table<T>().Where(predicate).CountAsync();
        }

        public async Task<List<T>> GetItemsAsync<T>(int page) where T : ICelestialBody, new() => await GetItemsAsync<T>(page, "Id", true);
        public async Task<List<T>> GetItemsAsync<T>(int page, string sortBy, bool asc = true) where T : ICelestialBody, new() => await GetItemsAsync<T>(page, sortBy, asc, t => true);
        public async Task<List<T>> GetItemsAsync<T>(int page, string sortBy, bool asc, Expression<Func<T, bool>> predicate) where T : ICelestialBody, new()
        {
            await Init();
            var query = database.Table<T>().Where(predicate);

            var parameter = Expression.Parameter(typeof(T), "t");
            var sortExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(parameter, sortBy), typeof(object)), parameter);

            if (asc) query = query.OrderBy(sortExpression);
            else query = query.OrderByDescending(sortExpression);

            return await query.Skip(page * Constants.PageSize).Take(Constants.PageSize).ToListAsync();
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
