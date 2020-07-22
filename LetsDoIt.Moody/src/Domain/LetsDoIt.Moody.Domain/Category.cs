using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Domain
{
   public class Category
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
