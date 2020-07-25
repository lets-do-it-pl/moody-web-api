namespace LetsDoIt.Moody.Application.Category
{
    public interface ICategoryService
    {
        Task UpdateAsync(int id, string name, int order, byte[] image);
    }
}
