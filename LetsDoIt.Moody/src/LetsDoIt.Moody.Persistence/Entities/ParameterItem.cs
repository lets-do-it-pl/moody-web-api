using System;

namespace LetsDoIt.Moody.Persistence.Entities
{
    public partial class ParameterItem
    {
        public int Id { get; set; }
        public string ParameterKey { get; set; }
        public string ParameterValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

    }
}
