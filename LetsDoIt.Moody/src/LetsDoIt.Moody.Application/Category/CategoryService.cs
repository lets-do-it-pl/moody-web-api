using NGuard;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Application.Category
{
    using VersionHistory;
    using Persistence.Entities;
    using Persistence.Repositories.Base;
    using Persistence.Repositories.Category;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRepository<CategoryDetail> _categoryDetailsRepository;
        private readonly IVersionHistoryService _versionHistoryService;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IRepository<CategoryDetail> categoryDetailsRepository,
            IVersionHistoryService versionHistoryService,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _categoryDetailsRepository = categoryDetailsRepository;
            _versionHistoryService = versionHistoryService;
            _logger = logger;
        }

        public async Task<CategoryGetResult> GetCategoriesWithDetails(string versionNumber)
        {
            var latestVersion = await _versionHistoryService.GetLatestVersionNumberAsync();

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

            result.Categories = await _categoryRepository.GetListWithDetailsAsync();

            return result;
        }

        public async Task InsertAsync(string name, int order, byte[] image)
        {
            _logger.LogInformation($"{nameof(InsertAsync)} executing with " +
                                   $"name={name};" +
                                   $"order={order};" +
                                   "image");

            await _categoryRepository.AddAsync(new Category
            {
                Name = name,
                Order = order,
                Image = image
            });

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(InsertAsync)} executed");
        }

        public async Task InsertCategoryDetailsAsync(int categoryId, int order, string image)
        {
            _logger.LogInformation($"{nameof(InsertCategoryDetailsAsync)} executing with " +
                                   $"categoryId={categoryId};" +
                                   $"order={order};" +
                                   "image");

            await _categoryDetailsRepository.AddAsync(new CategoryDetail
            {
                CategoryId = categoryId,
                Order = order,
                Image = Convert.FromBase64String(image)
            });

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(InsertCategoryDetailsAsync)} executed");
        }

        public async Task UpdateAsync(int id, string name, int order, byte[] image)
        {
            _logger.LogInformation($"{nameof(UpdateAsync)} executing with " +
                                   $"id={id};" +
                                   $"name={name};" +
                                   $"order={order};" +
                                   "image");

            var entity = await _categoryRepository.SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            entity.Name = name;
            entity.Order = order;
            entity.Image = image;

            await _categoryRepository.UpdateAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(UpdateAsync)} executed");
        }

        public async Task UpdateCategoryDetailsAsync(int id, int order, byte[] image)
        {
            _logger.LogInformation($"{nameof(UpdateCategoryDetailsAsync)} executing with " +
                                   $"id={id};" +
                                   $"order={order};" +
                                   "image");

            var entity = await _categoryDetailsRepository.SingleOrDefaultAsync(detail => detail.Id == id && !detail.IsDeleted);

            entity.Order = order;
            entity.Image = image;

            await _categoryDetailsRepository.UpdateAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(UpdateCategoryDetailsAsync)} executed");
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"{nameof(DeleteAsync)} executing with id={id}");

            var category = await _categoryRepository.SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            await _categoryRepository.DeleteAsync(category);

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(DeleteAsync)} executed");
        }

        public async Task DeleteCategoryDetailsAsync(int id)
        {
            _logger.LogInformation($"{nameof(DeleteCategoryDetailsAsync)} executing with id={id}");

            var entity = await _categoryDetailsRepository.SingleOrDefaultAsync(detail => detail.Id == id && !detail.IsDeleted);

            await _categoryDetailsRepository.DeleteAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(DeleteCategoryDetailsAsync)} executed");
        }
    }
}