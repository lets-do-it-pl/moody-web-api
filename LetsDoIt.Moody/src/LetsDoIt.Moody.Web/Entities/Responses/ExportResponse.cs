using System;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class ExportResponse
    {
        public string Name { get; set; }

        public int ImageCount { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
