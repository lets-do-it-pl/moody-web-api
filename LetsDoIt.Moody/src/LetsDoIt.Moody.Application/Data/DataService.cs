namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
