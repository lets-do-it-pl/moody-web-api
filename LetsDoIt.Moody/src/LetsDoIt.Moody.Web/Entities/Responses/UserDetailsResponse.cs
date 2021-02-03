using System;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class UserDetailsResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public bool CanLogin { get; set; }

        public string UserType { get; set; }
    }
}
