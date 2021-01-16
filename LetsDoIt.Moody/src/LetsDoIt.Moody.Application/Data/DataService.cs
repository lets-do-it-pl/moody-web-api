namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using System.Threading.Tasks;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;

    public class DataService : IDataService , IAsyncEnumerable<string> 
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<string>> SearchFunctionDatabese(string searchKey)
        {
            List<string> fakeDatabase = new List<string>();
            fakeDatabase.Add("adam");
            fakeDatabase.Add("inek");
            fakeDatabase.Add("koyun");
            fakeDatabase.Add("kurbaga");
            fakeDatabase.Add("at");
            fakeDatabase.Add("salih");
            fakeDatabase.Add("samed");
            fakeDatabase.Add("selim");

            var linq =  fakeDatabase.Where(p => p.Contains(searchKey));



            return await Task.FromResult(linq.ToList());

        }
    }
}
