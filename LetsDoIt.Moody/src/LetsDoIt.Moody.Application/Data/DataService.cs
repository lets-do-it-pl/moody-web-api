namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using System.Threading.Tasks;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<string> SearchFunction(string searchKey)
        {
            throw new System.NotImplementedException();
        }
    }
}
