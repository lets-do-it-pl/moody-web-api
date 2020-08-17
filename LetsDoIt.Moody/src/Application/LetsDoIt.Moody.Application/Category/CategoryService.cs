using System.Linq;
using System.Threading.Tasks;
using NGuard;

namespace LetsDoIt.Moody.Application.Category
{
    using Domain;
    using CustomExceptions;
    using Persistance.Repositories.Base;
    using VersionHistory;

    public class CategoryService : ICategoryService
    {
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly IEntityRepository<VersionHistory> _versionHistoryRepository;
        private readonly IVersionHistoryService _versionHistoryService;

        public CategoryService(
            IEntityRepository<Category> categoryRepository,
            IEntityRepository<VersionHistory> versionHistoryRepository,
            IVersionHistoryService versionHistoryService)
        {
            _categoryRepository = categoryRepository;
            _versionHistoryRepository = versionHistoryRepository;
            _versionHistoryService = versionHistoryService;
        }

        public async Task<CategoryGetResult> GetCategories(string versionNumber)
        {
            var latestVersion = _versionHistoryRepository.Get().OrderByDescending(vh => vh.CreateDate).FirstOrDefault();
            Guard.Requires(latestVersion, nameof(latestVersion));

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
    }
}