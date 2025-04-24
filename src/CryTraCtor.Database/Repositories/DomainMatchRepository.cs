using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories
{
    public class DomainMatchRepository(
        IDbContextFactory<CryTraCtorDbContext> dbContextFactory
    ) : IDomainMatchRepository
    {
        public async Task InsertAsync(DomainMatchEntity entity)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Set<DomainMatchEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<DomainMatchEntity> entities)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Set<DomainMatchEntity>().AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
        }
    }
}
