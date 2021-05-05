using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;
using System.IO;

namespace Dictionary.Database
{
    class ResultsDatabase
    {
        public static SQLiteAsyncConnection Database;

        public async static Task<ResultsDatabase> CreateDatabase()
		{
            var instance = new ResultsDatabase();
            CreateTableResult result = await Database.CreateTableAsync<Result>();
            return instance;
        }

        public ResultsDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public async Task<List<Result>> GetResultsAsync()
        {
            return await Database.Table<Result>().ToListAsync();
        }

        public async Task<Result> GetResultAsync(int id)
        {
            return await Database.Table<Result>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveResultAsync(Result item)
        {
            if (item.ID != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(Result item)
        {
            return await Database .DeleteAsync(item);
        }
    }
}