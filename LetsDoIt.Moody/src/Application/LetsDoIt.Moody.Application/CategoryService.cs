using System;
using Dawn;


namespace LetsDoIt.Moody.Application
{
    using Persistance;

    class CategoryService : ICategoryService
    {
        // this part will be updated with repository pattern
        var context = new ApplicationContext();

        void Update(int id, string name, int order, byte[] image) {
            throw new NotImplementedException();
        }
        public void GetCategory(string versionNumber)
        {

            // nGuard - Throws if versionNumber is null or empty
            Guard.Argument(versionNumber, nameof(versionNumber)).NotNull().NotEmpty();

            // Check the last versionNumber
            var result = context.VersionHistories.OrderByDescending(vh => vh.CreateDate).FirstOrDefault();

            Guard.Argument(result, nameof(result)).NotNull(); // check if the result(dateTime) is not null or empty

            var isUpdated = result.VersionNumber == versionNumber;

            if (!isUpdated)
            {
                //Find all not deleted categories
                var notDeletedCategories = context.Categories.Where(c => !c.isDeleted).ToList();
            }

        }
    }
}