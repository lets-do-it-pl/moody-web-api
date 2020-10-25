﻿using System.ComponentModel.DataAnnotations;
using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class SaveSystemUserRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public UserTypes UserType { get; set; } = UserTypes.Normal;

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }
    }
}