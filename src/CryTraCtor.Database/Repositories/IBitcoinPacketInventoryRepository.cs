using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public interface IBitcoinPacketInventoryRepository
{
    IQueryable<BitcoinPacketInventoryEntity> Get();
    Task<BitcoinPacketInventoryEntity> InsertAsync(BitcoinPacketInventoryEntity entity);
    Task DeleteAsync(Guid bitcoinPacketId, Guid bitcoinInventoryId);
}
