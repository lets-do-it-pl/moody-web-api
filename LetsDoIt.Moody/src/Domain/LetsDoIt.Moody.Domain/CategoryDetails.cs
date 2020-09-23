using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDoIt.Moody.Domain
{
    public class CategoryDetails : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Domain.Category))]
        public int CategoryId { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Category Category { get; set; }
    }
}
