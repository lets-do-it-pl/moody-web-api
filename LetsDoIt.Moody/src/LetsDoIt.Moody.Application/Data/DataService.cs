namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using System.Threading.Tasks;
    using System.Collections;
    using System.Collections.Generic;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> SearchFunctionDatabese(string searchKey)
        {
            List<string> fakeDatabase = new List<string>();
            fakeDatabase.Add("adam");
            fakeDatabase.Add("inek");
            fakeDatabase.Add("koyun");
            fakeDatabase.Add("kurbaga");
            fakeDatabase.Add("at");

            

            return fakeDatabase;

        }
    }
}
