using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;

namespace CryTraCtor.Database.Repositories;

public class BitcoinPacketRepository(CryTraCtorDbContext dbContext, IEntityMapper<BitcoinPacketEntity> entityMapper)
    : Repository<BitcoinPacketEntity>(dbContext, entityMapper), IBitcoinPacketRepository
{
}
