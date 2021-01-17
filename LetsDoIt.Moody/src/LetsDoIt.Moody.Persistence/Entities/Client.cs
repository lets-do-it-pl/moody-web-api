using System;

namespace LetsDoIt.Moody.Persistence.Entities
{
    public class Client
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
