namespace LetsDoIt.Moody.Application.Services.Abstract
{
    public interface ICategoryService
    {
        void Update(int id, string name, int order, byte[] image);
    }
}
