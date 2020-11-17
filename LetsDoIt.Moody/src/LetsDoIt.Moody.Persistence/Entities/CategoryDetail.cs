using System;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Persistence.Entities
{
    public partial class CategoryDetail
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int Order { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category Category { get; set; }
    }
}
