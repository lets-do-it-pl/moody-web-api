namespace LetsDoIt.Moody.Application.Category
{
    using Domain;
    using Persistance.Repositories.Base;
    using System;
    using System.Threading.Tasks;
    using VersionHistory;

    public class CategoryService : ICategoryService
    {
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly IVersionHistoryService _versionHistoryService;

        public CategoryService(
            IEntityRepository<Category> categoryRepository,
            IVersionHistoryService versionHistoryService)
        {
            _categoryRepository = categoryRepository;
            _versionHistoryService = versionHistoryService;
        }

      

        public async Task DeleteAsync(int id)
        {
            var entity = await _categoryRepository.GetAsync(c => c.Id == id);
            if (entity == null)
            {
                throw new Exception($"Category couldn't be found with id({id})");
            }


            await _categoryRepository.UpdateAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();
        }
    }
}

