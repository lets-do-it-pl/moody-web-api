using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category
{
    using Domain;
    using LetsDoIt.Moody.Application.CustomExceptions;
    using Persistance.Repositories.Base;
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
                throw new ObjectNotFoundException("Category", id);
            }
            await _categoryRepository.DeleteAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();
        }

        public async Task InsertAsync(string name, int order, byte[] image)
        {
            await _categoryRepository.AddAsync(new Category
            {
                Name = name,
                Order = order,
                Image = image
            });

            await _versionHistoryService.CreateNewVersionAsync();
        }

        public async Task UpdateAsync(int id, string name, int order, byte[] image)
        {
            var entity = await _categoryRepository.GetAsync(c => c.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category", id);
            }

            entity.Name = name;
            entity.Order = order;
            entity.Image = image;

            await _categoryRepository.UpdateAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();
        }
    }
}