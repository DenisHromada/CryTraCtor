using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories.Interfaces;

public interface IBitcoinMessageInventoryRepository
{
    IQueryable<BitcoinMessageInventoryEntity> Get();
    Task<BitcoinMessageInventoryEntity> InsertAsync(BitcoinMessageInventoryEntity entity);
    Task DeleteAsync(Guid bitcoinMessageId, Guid bitcoinInventoryId);
}
