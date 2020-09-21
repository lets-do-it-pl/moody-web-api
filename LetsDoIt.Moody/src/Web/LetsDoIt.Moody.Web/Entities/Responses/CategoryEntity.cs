using System.Collections.Generic;
using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public byte[] Image { get; set; }

    }
}
