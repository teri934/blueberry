using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Dictionary.Database
{
    class ResultsDatabase
    {
        public static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<ResultsDatabase> Instance = new AsyncLazy<ResultsDatabase>(async () =>
        {
            var instance = new ResultsDatabase();
            CreateTableResult result = await Database.CreateTableAsync<Result>();
            return instance;
        });

        public async static void CreateDatabase()
		{
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            CreateTableResult result = await Database.CreateTableAsync<Result>();
        }

        public ResultsDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<Result>> GetResultsAsync()
        {
            return Database.Table<Result>().ToListAsync();
        }

        public Task<Result> GetResultAsync(int id)
        {
            return Database.Table<Result>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveResultAsync(Result item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Result item)
        {
            return Database.DeleteAsync(item);
        }
    }

    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        readonly Lazy<Task<T>> instance;

        public AsyncLazy(Func<T> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }
    }
}