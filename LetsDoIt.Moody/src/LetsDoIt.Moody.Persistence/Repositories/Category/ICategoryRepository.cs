using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistence.Repositories.Category
{
    using Base;
    using Entities;

    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetListWithDetailsAsync();
    }
}
