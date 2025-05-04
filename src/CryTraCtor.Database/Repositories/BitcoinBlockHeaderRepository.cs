using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class BitcoinBlockHeaderRepository(
    CryTraCtorDbContext dbContext,
    IEntityMapper<BitcoinBlockHeaderEntity> entityMapper)
    : Repository<BitcoinBlockHeaderEntity>(dbContext, entityMapper), IBitcoinBlockHeaderRepository
{
    public async Task<BitcoinBlockHeaderEntity> GetOrCreateByBlockHashAsync(string blockHash,
        BitcoinBlockHeaderEntity newHeaderEntity)
    {
        var existingHeader = await Get().FirstOrDefaultAsync(h => h.BlockHash == blockHash);

        if (existingHeader != null)
        {
            return existingHeader;
        }
        else
        {
            if (newHeaderEntity.Id == Guid.Empty)
            {
                newHeaderEntity.Id = Guid.NewGuid();
            }

            await InsertAsync(newHeaderEntity);

            return newHeaderEntity;
        }
    }


    public IQueryable<BitcoinBlockHeaderEntity> Get() => base.Get();
    public IQueryable<BitcoinBlockHeaderEntity> GetLocal() => base.GetLocal();
    public ValueTask<bool> ExistsAsync(BitcoinBlockHeaderEntity entity) => base.ExistsAsync(entity);
    public Task<BitcoinBlockHeaderEntity> InsertAsync(BitcoinBlockHeaderEntity entity) => base.InsertAsync(entity);
    public Task<BitcoinBlockHeaderEntity> UpdateAsync(BitcoinBlockHeaderEntity entity) => base.UpdateAsync(entity);
    public Task DeleteAsync(Guid entityId) => base.DeleteAsync(entityId);
}
