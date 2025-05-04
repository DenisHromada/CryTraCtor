using CryTraCtor.Business.Models.Bitcoin;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IBitcoinInventoryFacade
{
    Task<BitcoinInventoryItemModel?> GetModelByIdAsync(Guid inventoryId);
}
