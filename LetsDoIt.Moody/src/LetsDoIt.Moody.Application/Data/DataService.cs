using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkExtras.EFCore;

namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using Persistence.StoredProcedures;
    using Persistence.StoredProcedures.ResultEntities;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<SpGetGeneralSearchResult>> GetGeneralSearchResultAsync(string searchKey)
        {
            var input = new SpGeneralSearch { SearchValue = searchKey };

            var result = await _dbContext.Database.ExecuteStoredProcedureAsync<SpGetGeneralSearchResult>(input);

            if (result == null)
            {
                throw new NullReferenceException("NULL Search Result");
            }

            return result.ToArray();
        }
        public async Task<ICollection<SpGetDashboardItemsResult>> GetDashboardItemsAsync()
        {
            var input = new SpGetDashboardItems { };

            var result = await _dbContext.Database.ExecuteStoredProcedureAsync<SpGetDashboardItemsResult>(input);

            if (result == null)
            {
                return null;
            }

            return result.ToArray();
        }

        public IEnumerable<CategoryUserReturnResult> GetCategoriesWithUsers()
        {
            var result = from category in _dbContext.Categories
                         join createdBy in _dbContext.Users
                         on category.CreatedBy equals createdBy.Id
                         join modifiedBy in _dbContext.Users
                         on category.ModifiedBy equals modifiedBy.Id into modifiedByUsers
                         from modifiedByUser in modifiedByUsers.DefaultIfEmpty()
                         select new
                         {
                             CategoryId = category.Id,
                             CategoryName = category.Name,
                             CategoryDetailsImageCount = category.CategoryDetails.Count,
                             CreatedDate = category.CreatedDate,
                             ModifiedDate = category.ModifiedDate,
                             CreatedBy = createdBy.FullName,
                             ModifiedBy = modifiedByUser.FullName

                         };

            foreach(var category in result)
            {
                yield return new CategoryUserReturnResult
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                    Image = category.CategoryDetailsImageCount,
                    CreatedDate = category.CreatedDate,
                    ModifiedDate = category.ModifiedDate,
                    CreatedBy = category.CreatedBy,
                    ModifiedBy = category.ModifiedBy
                };
            }
        }
    }
}