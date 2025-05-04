using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public interface IBitcoinPacketRepository : IRepository<BitcoinMessageEntity>
{
    Task<IEnumerable<BitcoinMessageEntity>> GetByInventoryIdAndAnalysisIdAsync(Guid inventoryId, Guid fileAnalysisId);
}
