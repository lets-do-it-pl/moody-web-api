namespace LetsDoIt.Moody.Application
{
    public interface ICategoryService
    {
        void Update(int id, string name, int order, byte[] image);
    }
}
