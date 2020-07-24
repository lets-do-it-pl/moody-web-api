namespace LetsDoIt.Moody.Application.Services.CategoryFolder
{
    public interface ICategoryService
    {
        void Update(int id, string name, int order, byte[] image);
    }
}
