using System;
using System.Collections.Generic;
using System.Text;

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