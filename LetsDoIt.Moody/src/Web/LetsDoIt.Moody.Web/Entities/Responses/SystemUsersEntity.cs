using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Infrastructure.ValueTypes;


namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class SystemUsersEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public Email Email { get; set; }

        public bool IsActive { get; set; }

        public UserType UserType { get; set; }

        
    }
}
