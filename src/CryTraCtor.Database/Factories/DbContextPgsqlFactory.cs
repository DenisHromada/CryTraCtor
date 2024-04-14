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
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString ?? string.Empty);
        var dataSource = dataSourceBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<CryTraCtorDbContext>();
        optionsBuilder.UseNpgsql(dataSource);

        _contextPool = new PooledDbContextFactory<CryTraCtorDbContext>(optionsBuilder.Options);
    }

    public DbContextPgsqlFactory(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<CryTraCtorDbContext>();
        optionsBuilder.UseNpgsql(dataSource);

        _contextPool = new PooledDbContextFactory<CryTraCtorDbContext>(optionsBuilder.Options);
    }

    public CryTraCtorDbContext CreateDbContext() => _contextPool.CreateDbContext();

    public async Task<CryTraCtorDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
        => await _contextPool.CreateDbContextAsync(cancellationToken);
}