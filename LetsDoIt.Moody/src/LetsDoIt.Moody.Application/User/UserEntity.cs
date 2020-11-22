﻿using System;
using System.Collections.Generic;
using System.Text;
using LetsDoIt.Moody.Application.Constants;
using LetsDoIt.Moody.Infrastructure.Utils;

namespace LetsDoIt.Moody.Application.User
{
    public class UserEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string UserType { get; set; }

        public UserEntity(string username,
            string password,
            string name,
            string surname,
            string email,
            bool isActive = false,
            string userType =UserTypeConstants.Standard)
        {
            Username = username;
            Password = ProtectionHelper.EncryptValue(username + password);
            FullName = name + surname;
            Email = email;
            IsActive = isActive;
            UserType = userType;
        }
    }
}
