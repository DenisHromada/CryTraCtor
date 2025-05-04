using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Database.UnitOfWork;

namespace CryTraCtor.Business.Facades;

public class BitcoinInventoryFacade(
    IUnitOfWorkFactory unitOfWorkFactory
) : IBitcoinInventoryFacade
{
    public async Task<BitcoinInventoryItemModel?> GetModelByIdAsync(Guid inventoryId)
    {
        await using var uow = unitOfWorkFactory.Create();
        var inventoryRepository = uow.BitcoinInventories;

        var entity = await inventoryRepository.GetByIdAsync(inventoryId);

        if (entity == null)
        {
            return null;
        }

        return new BitcoinInventoryItemModel
        {
            Id = entity.Id,
            Type = entity.Type,
            Hash = entity.Hash
        };
    }
}
