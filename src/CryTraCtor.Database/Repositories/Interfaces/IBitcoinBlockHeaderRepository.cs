using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories.Interfaces;

public interface IBitcoinBlockHeaderRepository : IRepository<BitcoinBlockHeaderEntity>
{
    Task<BitcoinBlockHeaderEntity> GetOrCreateByBlockHashAsync(string blockHash, BitcoinBlockHeaderEntity newHeaderEntity);
}
