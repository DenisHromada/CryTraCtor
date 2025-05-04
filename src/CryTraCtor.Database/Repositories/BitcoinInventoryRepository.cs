using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class BitcoinInventoryRepository(
    CryTraCtorDbContext dbContext,
    IEntityMapper<BitcoinInventoryEntity> entityMapper)
    : Repository<BitcoinInventoryEntity>(dbContext, entityMapper), IBitcoinInventoryRepository
{
    public async Task<BitcoinInventoryEntity> GetOrCreateAsync(string type, string hash)
    {
        var existingEntity = await Get().FirstOrDefaultAsync(inv => inv.Type == type && inv.Hash == hash);

        if (existingEntity != null)
        {
            return existingEntity;
        }

        var newEntity = new BitcoinInventoryEntity
        {
            Id = Guid.NewGuid(),
            Type = type,
            Hash = hash
        };

        await InsertAsync(newEntity);


        return newEntity;
    }

    public async Task<BitcoinInventoryEntity?> GetByIdAsync(Guid inventoryId)
    {
        return await Get().FirstOrDefaultAsync(inv => inv.Id == inventoryId);
    }
}
