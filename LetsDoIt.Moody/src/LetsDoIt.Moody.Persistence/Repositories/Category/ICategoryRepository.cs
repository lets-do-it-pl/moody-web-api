using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistence.Repositories.Category
{
    using Base;

    public interface ICategoryRepository : IRepository<Entities.Category>
    {
        Task<List<Entities.Category>> GetListWithDetailsAsync();
    }
}
