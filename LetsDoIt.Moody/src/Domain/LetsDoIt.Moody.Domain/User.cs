using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDoIt.Moody.Domain
{
    public class User:IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime CreateDate { get; set; } //Date will be created automatically
        public DateTime? ModifiedDate { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
