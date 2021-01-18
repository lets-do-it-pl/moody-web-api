using System;
using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class OrderUpdateRequest
    {
        [Required]
        public int PreviousId { get; set; }

        [Required]
        public int NextId { get; set; }
    }
}
