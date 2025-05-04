using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public class BitcoinPacketInventoryRepository(CryTraCtorDbContext dbContext) : IBitcoinPacketInventoryRepository
{
    public IQueryable<BitcoinPacketInventoryEntity> Get() => dbContext.Set<BitcoinPacketInventoryEntity>();

    public async Task<BitcoinPacketInventoryEntity> InsertAsync(BitcoinPacketInventoryEntity entity)
    {
        await dbContext.Set<BitcoinPacketInventoryEntity>().AddAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(Guid bitcoinPacketId, Guid bitcoinInventoryId)
    {
        var entity = await dbContext.Set<BitcoinPacketInventoryEntity>().FindAsync(bitcoinPacketId, bitcoinInventoryId);
        if (entity != null)
        {
            dbContext.Set<BitcoinPacketInventoryEntity>().Remove(entity);
        }
    }
}
