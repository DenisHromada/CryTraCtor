using CryTraCtor.Business.Models.Bitcoin;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IBitcoinInventoryFacade
{
    Task<BitcoinInventoryItemListModel?> GetModelByIdAsync(Guid inventoryId);
    Task<BitcoinInventoryItemDetailModel?> GetDetailModelByIdAsync(Guid inventoryId, Guid fileAnalysisId);
}
