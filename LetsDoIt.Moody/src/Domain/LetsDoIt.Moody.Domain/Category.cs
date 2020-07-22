using System;

namespace LetsDoIt.Moody.Domain
{
	public class Category
	{
		public Category()
		{ }

		public long Id { get; set; }
		public string Name { get; set; }
		public int Order { get; set; }
		public byte[] Image { get; set; }
		public DateTime CreateDate { get; } //It should be generated automatically
		public DateTime ModifiedDate { get; }
		public bool isDeleted { get; set; }  //(False) It should be default value in the db table

	}
}