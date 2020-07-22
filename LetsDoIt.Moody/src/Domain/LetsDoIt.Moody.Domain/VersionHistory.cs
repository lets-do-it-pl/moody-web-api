using System;
namespace LetsDoIt.Moody.Domain
{
	public class VersionHistory
	{
		public VersionHistory()
		{
		}
		public long Id { get; set; }
		public int VersionNumber { get; set; }
		public DateTime CreateDate { get; } //It should be generated automatically


	}
}