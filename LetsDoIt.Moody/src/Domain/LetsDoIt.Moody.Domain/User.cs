using System;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Domain
{
    public class User
    {
        public string Id { get; set; }
        public string UsernameAndPassword { get; set; }
        public DateTime CreateDate { get; } 
        public bool isDeleted { get; set; }

    }
}
