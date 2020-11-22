using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Application.User
{
    class UserEntity
    {
       

        public UserEntity(string username, string password, string fullname, string email, bool isActive, string userType, int createdBy)
        {
            Username = username;
            Password = password;
            Fullname = fullname;
            Email = email;
            IsActive = isActive;
            UserType = userType;
            CreatedBy = createdBy;
        }

        public string Username { get; }
        public string Password { get; }
        public string Fullname { get; }
        public string Email { get; }
        public bool IsActive { get; }
        public string UserType { get; }
        public int CreatedBy { get; }
    }
}
