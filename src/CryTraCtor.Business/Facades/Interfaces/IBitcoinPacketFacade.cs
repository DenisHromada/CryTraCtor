using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IBitcoinPacketFacade : IFacade<BitcoinPacketEntity, BitcoinPacketListModel, BitcoinPacketDetailModel>
{

    Task<IEnumerable<BitcoinPacketDetailModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
    Task<IEnumerable<BitcoinPacketListModel>> GetPacketListByInventoryIdAndAnalysisIdAsync(Guid inventoryId, Guid fileAnalysisId);
}
