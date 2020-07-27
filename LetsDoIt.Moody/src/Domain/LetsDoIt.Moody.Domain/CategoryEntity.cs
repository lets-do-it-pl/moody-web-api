using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Domain
{
    public class CategoryEntity
    {
        public string VersionNumber { get; set; }
      
        public bool IsUpdated { get; set; }
        
        public Category Categories { get; set; }
    }
}
