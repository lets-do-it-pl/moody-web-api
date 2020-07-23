using System;
using LetsDoIt.Moody.Domain.Entities.Abstract;

namespace LetsDoIt.Moody.Domain.Entities
{
    public class User:IEntity
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; } //Date will be created automatically
        public bool isDeleted { get; set; } = false;
    }
}
