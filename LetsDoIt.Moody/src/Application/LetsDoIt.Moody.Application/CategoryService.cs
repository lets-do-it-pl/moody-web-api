using System;
using Dawn;


namespace LetsDoIt.Moody.Application
{
    using Persistance;
    using Domain;

    class CategoryService : ICategoryService
    {
        // this part will be updated with repository pattern
        var context = new ApplicationContext();

        void Update(int id, string name, int order, byte[] image) {
            throw new NotImplementedException();
        }
        public CategoryEntity GetCategory(string versionNumber)
        {
            // if versionNumber is null (happens only in first call from mobil side) return all categories
            if(string.IsNullOrEmpty(versionNumber))
            {
                return context.CategoryEntities.Where(ce => ce.VersionNumber == versionNumber);
            }

            // Check the last versionNumber
            var result = context.VersionHistories.OrderByDescending(vh => vh.CreateDate).FirstOrDefault();
            Guard.Argument(result, nameof(result)).NotNull(); // check if the result(dateTime) is not null or empty
            var isUpdated = result.VersionNumber == versionNumber;

            if (!isUpdated) // NO - not latest
            {
                //Find all not deleted categories
                var notDeletedCategories = context.Categories.Where(c => !c.isDeleted).ToList();
                var notLatest = context.CategoriesEntities.Where(ce => ce.VersionNumber == versionNumber);
                notLatest.IsUpdated = false; // updated info - false
                context.SaveChanges(); 
                var returnResult = context.CategoryEntities.Where(ce => !ce.isUpdated && ce.Categories == notDeletedCategories);
                return returnResult;
            }
            // yes -  latest     
            var latest = context.CategoriesEntities.Where(ce => ce.VersionNumber == versionNumber);
            latest.IsUpdated = true;
            context.SaveChanges();
            return context.CategoriesEntities;


        }
    }
}