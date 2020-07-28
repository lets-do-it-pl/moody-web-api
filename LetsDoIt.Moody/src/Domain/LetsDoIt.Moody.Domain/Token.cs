using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDoIt.Moody.Domain
{
    [Table("UserTokens")]
    public class Token
    {
        public Token() { }

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [ForeignKey("Token")]
        public string Jwt { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
    
}
