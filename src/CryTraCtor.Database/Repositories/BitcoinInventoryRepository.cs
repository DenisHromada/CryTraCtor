using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class BitcoinInventoryRepository(
    CryTraCtorDbContext dbContext,
    IEntityMapper<BitcoinInventoryEntity> entityMapper)
    : Repository<BitcoinInventoryEntity>(dbContext, entityMapper), IBitcoinInventoryRepository
{
    public async Task<BitcoinInventoryEntity> GetOrCreateAsync(string type, string hash)
    {
        // Check local DbContext cache first for Added entities
        var locallyAddedEntity = dbContext.ChangeTracker.Entries<BitcoinInventoryEntity>()
            .FirstOrDefault(e => e.State == EntityState.Added &&
                                   e.Entity.Type == type &&
                                   e.Entity.Hash == hash)?
            .Entity;

        if (locallyAddedEntity != null)
        {
            return locallyAddedEntity; // Return the instance already added in this transaction
        }

        // If not found locally, check the database
        var existingEntity = await Get().FirstOrDefaultAsync(inv => inv.Type == type && inv.Hash == hash);

        if (existingEntity != null)
        {
            return existingEntity; // Return the instance found in the database
        }

        // If not found locally or in the database, create and add a new one
        var newEntity = new BitcoinInventoryEntity
        {
            Id = Guid.NewGuid(),
            Type = type,
            Hash = hash
        };

        await InsertAsync(newEntity); // Add to DbContext tracking

        return newEntity; // Return the newly created and tracked entity
    }

    public async Task<BitcoinInventoryEntity?> GetByIdAsync(Guid inventoryId)
    {
        return await Get().FirstOrDefaultAsync(inv => inv.Id == inventoryId);
    }
}
