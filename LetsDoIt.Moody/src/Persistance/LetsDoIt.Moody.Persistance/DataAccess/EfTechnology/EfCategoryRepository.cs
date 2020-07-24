using System;
using System.Collections.Generic;
using System.Text;
using LetsDoIt.Moody.Domain.Entities;
using LetsDoIt.Moody.Persistance.DataAccess.EfTechnology;

namespace LetsDoIt.Moody.Persistance.DataAccess.EntityFramework
{
    public class EfCategoryRepository:EfEntityRepositoryBase<Category>,ICategoryRepository
    {
        public EfCategoryRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
