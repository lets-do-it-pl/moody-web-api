namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> SearchFunctionDatabase(string searchKey)
        {
            var fakeDatabase = new List<string>
            {
                "adam",
                "inek",
                "koyun",
                "kurbaga",
                "at",
                "salih",
                "samed",
                "selim"
            };

            var linq = fakeDatabase.Where(p => p.Contains(searchKey));

            return await Task.FromResult(linq.ToList());
        }
    }
}
