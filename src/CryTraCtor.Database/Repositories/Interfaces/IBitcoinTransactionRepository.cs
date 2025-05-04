using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories.Interfaces;

public interface IBitcoinTransactionRepository : IRepository<BitcoinTransactionEntity>
{
    Task<BitcoinTransactionEntity?> GetByTxIdAsync(string txId);
}
