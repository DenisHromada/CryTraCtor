using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public interface IBitcoinPacketRepository : IRepository<BitcoinPacketEntity>
{
    Task<IEnumerable<BitcoinPacketEntity>> GetByInventoryIdAndAnalysisIdAsync(Guid inventoryId, Guid fileAnalysisId);
}
