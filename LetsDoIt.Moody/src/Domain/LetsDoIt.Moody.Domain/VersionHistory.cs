using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDoIt.Moody.Domain
{
    public class VersionHistory
    {
        [Key]        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long Id { get; set; }

        [Required]
		public string VersionNumber { get; set; }
		
        [Required]
        public DateTime CreateDate { get; set; }
	}
}