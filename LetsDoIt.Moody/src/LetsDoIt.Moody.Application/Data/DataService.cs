using System;
using LetsDoIt.Moody.Application.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LetsDoIt.Moody.Application.Data
{
    using LetsDoIt.Moody.Application.CustomExceptions;
    using Persistence;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;
        ICollection<SearchGetResult> _searchResults;
        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<SearchGetResult>> GetSearchResultDbAsync(string searchKey)
        {
            _searchResults = new List<SearchGetResult>();

            var fakeDatabase = new List<Tuple<string, string>>
                            {
                                Tuple.Create("Category", "Turkiye"),
                                Tuple.Create("Category", "Tunus"),
                                Tuple.Create("User", "Turan Yilmaz"),
                                Tuple.Create("Client", "Salih Yavuz"),
                                Tuple.Create("Client", "Murat Temiz")
                            };

            var results = fakeDatabase.Where(p => p.Item2.ToLower().Contains(searchKey));

            var isResultExist = fakeDatabase.Any(u => u.Item2.ToLower().Contains(searchKey));
            
            if (!isResultExist)
            {
                throw new SearchResultNotFoundException(searchKey);
            }

            foreach (var r in results)
            {
                _searchResults.Add(new SearchGetResult
                {
                    Name = r.Item1,
                    Value = r.Item2
                });
            }

            return await Task.FromResult(_searchResults);
        }
    }
}
