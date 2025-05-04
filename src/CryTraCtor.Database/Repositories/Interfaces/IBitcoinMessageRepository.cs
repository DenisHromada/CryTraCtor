using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories.Interfaces;

public interface IBitcoinMessageRepository : IRepository<BitcoinMessageEntity>
{
    Task<IEnumerable<BitcoinMessageEntity>> GetByInventoryIdAndAnalysisIdAsync(Guid inventoryId, Guid fileAnalysisId);
}
