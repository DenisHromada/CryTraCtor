using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories.Interfaces;

public interface IBitcoinInventoryRepository : IRepository<BitcoinInventoryEntity>
{
    Task<BitcoinInventoryEntity> GetOrCreateAsync(string type, string hash);

    Task<BitcoinInventoryEntity?> GetByIdAsync(Guid inventoryId);
}
