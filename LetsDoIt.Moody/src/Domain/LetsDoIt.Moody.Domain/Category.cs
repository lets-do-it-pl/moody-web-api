using System;

namespace LetsDoIt.Moody.Domain
{
	public class Category
	{
		public Category()
		{ }

		public long id { get; set; }
		public string Name { get; set; }
		public int Order { get; set; }
		public byte[] Image { get; set; }
		public DateTime CreateDate { get; } //It should be generated automatically
		public DateTime ModifiedDate { get; }
		public bool isDeleted { get; set; }  //(False) It should be default value in the db table

		// entity.isDeleted = true;



		//whenever I'm trying to query an object from the Db, 
		//only include the ones where IsDeleted = false (they haven't been deleted)

		protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
           var typesSoftDelete = GetAllTypesWhichImplementIIsDeleted();
           foreach (var typ in typesSoftDelete)
           {
         modelBuilder.Entity<typ>().HasQueryFilter(p => !p.IsDeleted);
     
      }}




		// Marks any "Removed" Entities as "Modified" and then sets the Db [IsDeleted] Flag to true
		private override int SaveChanges()
		{
		ChangeTracker.DetectChanges();

		var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

		foreach (var item in markedAsDeleted)
		
		if (item.Entity is IIsDeleted entity)
		{
		// Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
		item.State = EntityState.Unchanged;
		// Only update the IsDeleted flag - only this will get sent to the Db
		entity.isDeleted = true;
    }
  }

	}
}