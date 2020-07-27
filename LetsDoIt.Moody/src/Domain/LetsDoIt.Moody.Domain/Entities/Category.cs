using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDoIt.Moody.Domain
{
    public class Category : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool isDeleted { get; set; }        
    }
}
