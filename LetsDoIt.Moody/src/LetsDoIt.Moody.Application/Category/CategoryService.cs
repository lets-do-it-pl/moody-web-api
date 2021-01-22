using NGuard;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Category
{
    using CustomExceptions;
    using VersionHistory;
    using Persistence.Entities;
    using Persistence.Repositories.Base;
    using Persistence.Repositories.Category;
    using Infrastructure.Utils;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRepository<CategoryDetail> _categoryDetailsRepository;
        private readonly IVersionHistoryService _versionHistoryService;
        private const int InitialOrder = 1000;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IRepository<CategoryDetail> categoryDetailsRepository,
            IVersionHistoryService versionHistoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryDetailsRepository = categoryDetailsRepository;
            _versionHistoryService = versionHistoryService;
        }

        public async Task<CategoryGetResult> GetCategoriesWithDetailsAsync(string versionNumber)
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


        public async Task InsertAsync(string name, byte[] image//, int userId
            )
        {
            var categoryList = await _categoryRepository.GetListAsync(c => !c.IsDeleted,c => c.Order);

            var order = categoryList.Count > 0 ?
                OrderGenerator.GenerateOrder(categoryList[categoryList.Count - 1].Order)
                : InitialOrder;

            await _categoryRepository.AddAsync(new Category
            {
                Name = name,
                Image = image,
                Order = order
                //CreatedBy = userId
            });

            await _versionHistoryService.CreateNewVersionAsync();
        }

        public async Task InsertCategoryDetailAsync(int categoryId, string image//, int userId
            )
        {
            var categoryDetailList = await _categoryDetailsRepository.GetListAsync(c => !c.IsDeleted, c => c.Order);

            var order = categoryDetailList.Count > 0 ?
                OrderGenerator.GenerateOrder(categoryDetailList[categoryDetailList.Count - 1].Order)
                : InitialOrder;

            await _categoryDetailsRepository.AddAsync(new CategoryDetail
            {
                CategoryId = categoryId,
                Image = Convert.FromBase64String(image),
                Order = order
                //CreatedBy = userId
            });

            await _versionHistoryService.CreateNewVersionAsync();

        }

        public async Task UpdateAsync(int id, string name, byte[] image//, int userId
            )
        {
            var entity = await _categoryRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category", id);
            }

            entity.Name = name;
            entity.Image = image;
            //entity.ModifiedBy = userId;

            await _categoryRepository.UpdateAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

        }

        public async Task UpdateCategoryDetailsAsync(int id, byte[] image//, int userId
            )
        {
            var entity = await _categoryDetailsRepository.GetAsync(detail => detail.Id == id && !detail.IsDeleted);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Category Detail", id);
            }

            entity.Image = image;
            //entity.ModifiedBy = userId;

            await _categoryDetailsRepository.UpdateAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

        }

        public async Task UpdateOrderAsync(int id, int? previousId, int? nextId//, int userId
            )
        {
            //var entity = await _categoryRepository.GetListAsync(c => c.Id == id ||
            //                                                c.Id == previousId ||
            //                                                c.Id == nextId);

            //if(entity[1] != null && entity[2] != null)
            //{
            //    entity[0].Order = (entity[1].Order + entity[2].Order) / 2;
            //}

            //if (entity[1] == null && entity[2] != null)
            //{
            //    entity[0].Order = entity[2].Order - 1;
            //}

            //if(entity[2] == null && entity[1] != null)
            //{
            //    entity[0].Order = entity[1].Order + 1;
            //}

            //await _categoryRepository.UpdateAsync(entity[0]);

            //await _versionHistoryService.CreateNewVersionAsync();

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

            category.Order = category.Order + (-1000);

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

            entity.Order = entity.Order + (-1000);

            await _categoryDetailsRepository.DeleteAsync(entity);

            await _versionHistoryService.CreateNewVersionAsync();

        }
    }
}