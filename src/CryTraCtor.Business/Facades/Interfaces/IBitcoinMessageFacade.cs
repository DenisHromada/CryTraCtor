using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IBitcoinMessageFacade : IFacade<BitcoinMessageEntity, BitcoinMessageListModel, BitcoinMessageDetailModel>
{

    Task<IEnumerable<BitcoinMessageDetailModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
    Task<IEnumerable<BitcoinMessageListModel>> GetPacketListByInventoryIdAndAnalysisIdAsync(Guid inventoryId, Guid fileAnalysisId);
}
