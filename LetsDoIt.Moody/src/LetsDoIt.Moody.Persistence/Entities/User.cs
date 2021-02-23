using System;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Persistence.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool CanLogin { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public bool IsDeleted { get; set; }
    }
}
