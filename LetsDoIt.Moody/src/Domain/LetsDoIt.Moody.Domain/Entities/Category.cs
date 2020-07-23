using System;
using LetsDoIt.Moody.Domain.Entities.Abstract;

namespace LetsDoIt.Moody.Domain.Entities
{
   public class Category:IEntity
    { 
        public Category() { }


        public long Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreateDate { get; } //Date will be created automatically
        public bool isDeleted { get; set; }

    }
}
