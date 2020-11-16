using NGuard;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Category
{
    using Domain;
    using VersionHistory;
    using CustomExceptions;
    using Persistance.Repositories.Base;

    public class CategoryService : ICategoryService
    {
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly IEntityRepository<CategoryDetails> _categoryDetailsRepository;
        private readonly IEntityRepository<VersionHistory> _versionHistoryRepository;
        private readonly IVersionHistoryService _versionHistoryService;


        public CategoryService(
            IEntityRepository<Category> categoryRepository,
            IEntityRepository<CategoryDetails> categoryDetailsRepository,
            IEntityRepository<VersionHistory> versionHistoryRepository,
            IVersionHistoryService versionHistoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryDetailsRepository = categoryDetailsRepository;
            _versionHistoryRepository = versionHistoryRepository;
            _versionHistoryService = versionHistoryService;
        }

        public async Task<CategoryGetResult> GetCategories(string versionNumber)
        {
            VersionHistory latestVersion = await _versionHistoryService.GetLatestVersionNumberAsync();

            Guard.Requires(latestVersion, nameof(latestVersion)).IsNotNull();
            Guard.Requires(latestVersion.VersionNumber, nameof(latestVersion.VersionNumber)).IsNotNullOrEmptyOrWhiteSpace();

            var result = new CategoryGetResult
            {
                IsUpdated = latestVersion.VersionNumber == versionNumber,
                VersionNumber = latestVersion.VersionNumber
            };

            if (result.IsUpdated)
            {
                return result;
            }

            result.Categories = await _categoryRepository.GetListAsync(c => !c.IsDeleted);

            return result;
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

        public async Task InsertCategoryDetailsAsync(int categoryId, int order, string image)
        {
            await _categoryDetailsRepository.AddAsync(new CategoryDetails
            {
                CategoryId = categoryId,
                Order = order,
                Image = Convert.FromBase64String(image)
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

        public async Task UpdateCategoryDetailsAsync(int id, int order, byte[] image)
        {
            var entity = await _categoryDetailsRepository.GetAsync(c => c.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category Detail", id);
            }

            entity.Id = id;
            entity.Order = order;
            entity.Image = image;

            await _categoryDetailsRepository.UpdateAsync(entity);
            await _versionHistoryService.CreateNewVersionAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            
            if (category == null)
            {
                throw new ObjectNotFoundException("Category", id);
            }

            await _categoryRepository.DeleteAsync(category);

            await _versionHistoryService.CreateNewVersionAsync();
        }

        public async Task DeleteCategoryDetailsAsync(int id)
        {
            var entity = await _categoryDetailsRepository.GetAsync(c => c.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category Detail", id);
            }
            await _categoryDetailsRepository.DeleteAsync(entity);
            await _versionHistoryService.CreateNewVersionAsync();
        }
    }
}