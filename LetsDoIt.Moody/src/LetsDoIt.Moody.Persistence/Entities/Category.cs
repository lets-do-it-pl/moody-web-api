using System;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Persistence.Entities
{
    public partial class Category
    {
        public Category()
        {
            CategoryDetails = new HashSet<CategoryDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Order { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<CategoryDetail> CategoryDetails { get; set; }
    }
}
