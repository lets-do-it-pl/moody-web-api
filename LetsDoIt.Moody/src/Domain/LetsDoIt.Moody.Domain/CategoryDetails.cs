using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDoIt.Moody.Domain
{
    public class CategoryDetails : IEntity
    {
        private Category _category;

        public CategoryDetails()
        {
        }

        private CategoryDetails(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private Action<object, string> LazyLoader { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key, ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public Category Categories
        {
            get => LazyLoader.Load(this, ref _category);
            set => _category = value;
        }
    }
}
