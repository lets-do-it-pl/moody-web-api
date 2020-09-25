using System;

public abstract class SqliteInMemoryItemsControllerTest : ItemsControllerTest, IDisposable
{
    private bool _useSqlite;

    public async Task<SampleDbContext> GetDbContext()
    {
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
        if (_useSqlite)
        {
            // Use Sqlite DB.
            builder.UseSqlite("DataSource=:memory:", x => { });
        }
        else
        {
            // Use In-Memory DB.
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()).
        }

        var dbContext = new SampleDbContext(builder.Options);
        if (_useSqlite)
        {
            // SQLite needs to open connection to the DB.
            // Not required for in-memory-database and MS SQL.
            await dbContext.Database.OpenConnectionAsync();
        }

        await dbContext.Database.EnsureCreatedAsync();

        return dbContext;
    }

    public void UseSqlite()
    {
        _useSqlite = true;
    }
}