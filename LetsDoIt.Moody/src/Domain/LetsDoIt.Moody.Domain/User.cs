using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LetsDoIt.Moody.Domain.ValueType;

namespace LetsDoIt.Moody.Domain
{
    public class User:IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Password { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; } 

        public virtual UserToken UserToken { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public Email Email { get; set; }

        public bool IsActive { get; set; }

        public UserType UserType { get; set; }
    }

    public enum UserType
    {
        Mobile,
        Normal,
        Admin
    }

}
