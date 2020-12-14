using NGuard;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Category
{
    using CustomExceptions;
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

        public async Task<CategoryGetResult> GetCategories()
        {
            var result = new CategoryGetResult();

            result.Categories = await _categoryRepository.GetListAsync(c => !c.IsDeleted);

            return result;
        }

        public async Task<IEnumerable<CategoryDetail>> GetCategoryDetails(int categoryId)
        {
            return await _categoryDetailsRepository.GetListAsync(
                c => !c.IsDeleted && c.CategoryId == categoryId);
        }


        public async Task InsertAsync(string name, decimal order, byte[] image//, int userId
            )
        {
            _logger.LogInformation($"{nameof(InsertAsync)} executing with " +
                                   $"name={name};" +
                                   $"order={order};" +
                                   "image");

            await _categoryRepository.AddAsync(new Category
            {
                Name = name,
                Order = order,
                Image = image,
                //CreatedBy = userId
            });

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(InsertAsync)} executed");
        }

        public async Task InsertCategoryDetailsAsync(int categoryId, decimal order, string image//, int userId
            )
        {
            _logger.LogInformation($"{nameof(InsertCategoryDetailsAsync)} executing with " +
                                   $"categoryId={categoryId};" +
                                   $"order={order};" +
                                   "image");

            await _categoryDetailsRepository.AddAsync(new CategoryDetail
            {
                CategoryId = categoryId,
                Order = order,
                Image = Convert.FromBase64String(image),
                //CreatedBy = userId
            });

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(InsertCategoryDetailsAsync)} executed");
        }

        public async Task UpdateAsync(int id, string name, decimal order, byte[] image//, int userId
            )
        {
            _logger.LogInformation($"{nameof(UpdateAsync)} executing with " +
                                   $"id={id};" +
                                   $"name={name};" +
                                   $"order={order};" +
                                   "image");

            var entity = await _categoryRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category", id);
            }

            entity.Name = name;
            entity.Order = order;
            entity.Image = image;
            //entity.ModifiedBy = userId;

            await _categoryRepository.UpdateAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(UpdateAsync)} executed");
        }

        public async Task UpdateCategoryDetailsAsync(int id, decimal order, byte[] image//, int userId
            )
        {
            _logger.LogInformation($"{nameof(UpdateCategoryDetailsAsync)} executing with " +
                                   $"id={id};" +
                                   $"order={order};" +
                                   "image");

            var entity = await _categoryDetailsRepository.GetAsync(detail => detail.Id == id && !detail.IsDeleted);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category Detail", id);
            }

            entity.Order = order;
            entity.Image = image;
            //entity.ModifiedBy = userId;

            await _categoryDetailsRepository.UpdateAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(UpdateCategoryDetailsAsync)} executed");
        }

        public async Task DeleteAsync(int categoryId//, int userId
            )
        {
            _logger.LogInformation($"{nameof(DeleteAsync)} executing with id={categoryId}");

            var category = await _categoryRepository.GetAsync(c => c.Id == categoryId && !c.IsDeleted);
            if (category == null)
            {
                throw new ObjectNotFoundException("Category", categoryId);
            }

            //category.ModifiedBy = userId;

            await _categoryRepository.DeleteAsync(category);

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(DeleteAsync)} executed");
        }

        public async Task DeleteCategoryDetailsAsync(int id//, int userId
            )
        {
            _logger.LogInformation($"{nameof(DeleteCategoryDetailsAsync)} executing with id={id}");

            var entity = await _categoryDetailsRepository.GetAsync(detail => detail.Id == id && !detail.IsDeleted);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category Detail", id);
            }

            //entity.ModifiedBy = userId;

            await _categoryDetailsRepository.DeleteAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

            _logger.LogInformation($"{nameof(DeleteCategoryDetailsAsync)} executed");
        }
    }
}