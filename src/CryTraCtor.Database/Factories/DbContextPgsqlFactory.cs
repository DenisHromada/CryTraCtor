using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Factories;

public class DbContextPgsqlFactory : IDbContextFactory<CryTraCtorDbContext>
{
    private readonly bool _seedData;
    private readonly DbContextOptionsBuilder<CryTraCtorDbContext> _contextOptionsBuilder = new();

    public DbContextPgsqlFactory(string connectionString, bool seedData = false)
    {
        _seedData = seedData;
        _contextOptionsBuilder.UseNpgsql(connectionString);
    }

    public DbContextPgsqlFactory(string connectionString)
    {
        throw new NotImplementedException();
    }


    public CryTraCtorDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedData);

}