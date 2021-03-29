using LetsDoIt.CustomValueTypes.Image;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class CategoryDetailsEntity
    {
        public int Id { get; set; }

        public decimal Order { get; set; }

        public Image Image { get; set; }
    }
}
