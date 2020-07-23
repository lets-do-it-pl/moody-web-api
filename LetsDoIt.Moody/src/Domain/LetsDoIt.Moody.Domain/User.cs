using System;

namespace LetsDoIt.Moody.Domain
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; } 
        public bool isDeleted { get; set; }

    }
}
