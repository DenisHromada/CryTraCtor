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
        return await context.Set<BitcoinTransactionEntity>()
            .Include(t => t.Inputs)
            .Include(t => t.Outputs)
            .FirstOrDefaultAsync(t => t.TxId == txId);
    }
}
