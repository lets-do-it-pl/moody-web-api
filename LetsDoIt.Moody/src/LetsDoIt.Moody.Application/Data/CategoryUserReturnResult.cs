using System;

namespace LetsDoIt.Moody.Application.Data
{
    public class CategoryUserReturnResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

}

