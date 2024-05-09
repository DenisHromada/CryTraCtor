using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CryTraCtor.Database.Factories;

public class DbContextPgsqlFactory : IDbContextFactory<CryTraCtorDbContext>
{
    private readonly PooledDbContextFactory<CryTraCtorDbContext> _contextPool;

    public DbContextPgsqlFactory(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Database connection string not found!");
            }
        }
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString ?? string.Empty);
        var dataSource = dataSourceBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<CryTraCtorDbContext>();
        optionsBuilder.UseNpgsql(dataSource);

        _contextPool = new PooledDbContextFactory<CryTraCtorDbContext>(optionsBuilder.Options);
    }

    public DbContextPgsqlFactory()
    {
        var contextOptionsBuilder = new DbContextOptionsBuilder<CryTraCtorDbContext>();
        
        // this is where the connection string would be specified.
        // there doesn't seem to be a reasonable way to use the connection string from config other than hard coding it
        
        // Not specifying the host is fine for creating new migrations
        // When applying migrations, the connection string must be provided using the --connection flag
        contextOptionsBuilder.UseNpgsql();
        
        _contextPool = new PooledDbContextFactory<CryTraCtorDbContext>(contextOptionsBuilder.Options);
    }

    public CryTraCtorDbContext CreateDbContext() => _contextPool.CreateDbContext();

    public async Task<CryTraCtorDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
        => await _contextPool.CreateDbContextAsync(cancellationToken);
}