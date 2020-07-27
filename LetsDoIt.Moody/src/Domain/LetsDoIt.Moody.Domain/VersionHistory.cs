<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Text;
=======
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
>>>>>>> create-update-category-API

namespace LetsDoIt.Moody.Domain
{
    public class VersionHistory
    {
<<<<<<< HEAD
		public VersionHistory()
		{
		}
		public long Id { get; set; }
		public string VersionNumber { get; set; }
		public DateTime CreateDate { get; } //It should be generated automatically
	}
}
=======
        [Key]        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long Id { get; set; }

        [Required]
		public string VersionNumber { get; set; }
		
        [Required]
        public DateTime CreateDate { get; set; }
	}
}
>>>>>>> create-update-category-API
