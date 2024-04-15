using Microsoft.EntityFrameworkCore.Design;

namespace CryTraCtor.Database.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CryTraCtorDbContext>
{
    private readonly DbContextPgsqlFactory _dbContextPgsqlFactory = new();
    public CryTraCtorDbContext CreateDbContext(string[] args) => _dbContextPgsqlFactory.CreateDbContext();
}