using System;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string UserType { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
