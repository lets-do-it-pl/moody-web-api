using System;

namespace LetsDoIt.Moody.Domain
{
    public class Token
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Jwt { get; set; }
        public string ExprationDate { get; set; }
        public DateTime CreateDate { get; }
    }
    
}
