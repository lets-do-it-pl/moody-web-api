﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDoIt.Moody.Domain
{

    public class Category : IEntity
    {
        private ICollection<CategoryDetails> _posts;

        public Category()
        {
        }

        private Category(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private Action<object, string> LazyLoader { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<CategoryDetails> CategoryDetail
        {
            get => LazyLoader.Load(this, ref _posts);
            set => _posts = value;
        }
    }
}
