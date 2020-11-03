using System;
namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class CategoryDetailsEntityWeb
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public byte[] Image { get; set; }

        public int CategoryId { get; set; }
    }
}
