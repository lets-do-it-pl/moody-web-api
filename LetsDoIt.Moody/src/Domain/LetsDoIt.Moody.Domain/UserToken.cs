﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDoIt.Moody.Domain
{
    public class UserToken
    {
        [Key, ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [MaxLength(1000)]
        public string Token { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}