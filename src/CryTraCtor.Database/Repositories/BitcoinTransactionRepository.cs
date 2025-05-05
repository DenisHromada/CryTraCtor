using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class BitcoinTransactionRepository(
    CryTraCtorDbContext context,
    IEntityMapper<BitcoinTransactionEntity> entityMapper)
    : Repository<BitcoinTransactionEntity>(context, entityMapper), IBitcoinTransactionRepository
{
    public async Task<BitcoinTransactionEntity?> GetByTxIdAsync(string txId)
    {
        // Check local DbContext cache first for Added entities
        var locallyAddedEntity = context.ChangeTracker.Entries<BitcoinTransactionEntity>()
            .FirstOrDefault(e => e.State == EntityState.Added && e.Entity.TxId == txId)?
            .Entity;

        if (locallyAddedEntity != null)
        {
            // Note: Navigation properties (Inputs, Outputs) might not be loaded for locally added entities
            // depending on how they were added. The calling code should be aware of this.
            return locallyAddedEntity; // Return the instance already added in this transaction
        }

        // If not found locally, check the database
        return await context.Set<BitcoinTransactionEntity>()
            .Include(t => t.Inputs)
            .Include(t => t.Outputs)
            .FirstOrDefaultAsync(t => t.TxId == txId); // Return the instance found in the database or null
    }
}
