using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IBitcoinPacketFacade : IFacade<BitcoinPacketEntity, BitcoinPacketListModel, BitcoinPacketDetailModel>
{

    Task<IEnumerable<BitcoinPacketDetailModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
}
