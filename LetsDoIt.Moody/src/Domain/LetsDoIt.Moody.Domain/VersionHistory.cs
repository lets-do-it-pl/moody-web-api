using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Domain
{
    public class VersionHistory
    {
		public VersionHistory()
		{
		}
		public long Id { get; set; }
		public string VersionNumber { get; set; }
		public DateTime CreateDate { get; } //It should be generated automatically
	}
}
