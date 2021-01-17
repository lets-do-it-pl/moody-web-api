using NGuard;
using System;
using System.Threading.Tasks;
<<<<<<< HEAD
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
=======
>>>>>>> master

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

        public CategoryService(
            ICategoryRepository categoryRepository,
            IRepository<CategoryDetail> categoryDetailsRepository,
            IVersionHistoryService versionHistoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryDetailsRepository = categoryDetailsRepository;
            _versionHistoryService = versionHistoryService;
        }

        public async Task<CategoryGetResult> GetCategoriesWithDetails(string versionNumber)
        {
            var latestVersionHistory = await _versionHistoryService.GetLatestVersionNumberAsync();

            Guard.Requires(latestVersionHistory, nameof(latestVersionHistory)).IsNotNull();
            Guard.Requires(latestVersionHistory.VersionNumber, nameof(latestVersionHistory.VersionNumber)).IsNotNullOrEmptyOrWhiteSpace();

            var result = new CategoryGetResult
            {
                IsUpdated = latestVersionHistory.VersionNumber == versionNumber,
                VersionNumber = latestVersionHistory.VersionNumber
            };

            if (result.IsUpdated)
            {
                return result;
            }

            result.Categories = await _categoryRepository.GetListWithDetailsAsync();

            return result;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetListAsync(c => !c.IsDeleted);
        }

        public async Task<IEnumerable<CategoryDetail>> GetCategoryDetailsAsync(int categoryId)
        {
            return await _categoryDetailsRepository.GetListAsync(
                c => !c.IsDeleted && c.CategoryId == categoryId);
        }


        public async Task InsertAsync(string name, decimal order, byte[] image//, int userId
            )
        {
            await _categoryRepository.AddAsync(new Category
            {
                Name = name,
                Order = order,
                Image = image,
                //CreatedBy = userId
            });

            await _versionHistoryService.CreateNewVersionAsync();
        }

<<<<<<< HEAD
        public async Task InsertCategoryDetailsAsync(int categoryId, decimal order, string image//, int userId
            )
=======
        public async Task InsertCategoryDetailAsync(int categoryId, int order, string image, int userId)
>>>>>>> master
        {

            await _categoryDetailsRepository.AddAsync(new CategoryDetail
            {
                CategoryId = categoryId,
                Order = order,
                Image = Convert.FromBase64String(image),
                //CreatedBy = userId
            });

            await _versionHistoryService.CreateNewVersionAsync();

        }

        public async Task UpdateAsync(int id, string name, decimal order, byte[] image//, int userId
            )
        {
          
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

        }

        public async Task UpdateCategoryDetailsAsync(int id, decimal order, byte[] image//, int userId
            )
        {

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

        }

        public async Task DeleteAsync(int categoryId//, int userId
            )
        {

            var category = await _categoryRepository.GetAsync(c => c.Id == categoryId && !c.IsDeleted);

            if (category == null)
            {
                throw new ObjectNotFoundException("Category", categoryId);
            }

            //category.ModifiedBy = userId;

            await _categoryRepository.DeleteAsync(category);

            await _versionHistoryService.CreateNewVersionAsync();

        }

        public async Task DeleteCategoryDetailsAsync(int id//, int userId
            )
        {

            var entity = await _categoryDetailsRepository.GetAsync(detail => detail.Id == id && !detail.IsDeleted);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category Detail", id);
            }

            //entity.ModifiedBy = userId;

            await _categoryDetailsRepository.DeleteAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

        }
    }
}