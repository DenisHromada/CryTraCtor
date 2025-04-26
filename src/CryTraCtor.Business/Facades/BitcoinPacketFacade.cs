using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Facades;

public class BitcoinPacketFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    BitcoinPacketModelMapper modelMapper,
    ILogger<BitcoinPacketFacade> logger)
    : FacadeBase<BitcoinPacketEntity, BitcoinPacketListModel, BitcoinPacketDetailModel, BitcoinPacketEntityMapper>(
        unitOfWorkFactory, modelMapper), IBitcoinPacketFacade
{
    public async Task<IEnumerable<BitcoinPacketDetailModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        try
        {
            await using var uow = UnitOfWorkFactory.Create();

            var repository = uow.GetRepository<BitcoinPacketEntity, BitcoinPacketEntityMapper>();

            var entities = await repository.Get()
                .Include(p => p.Sender)
                .Include(p => p.Recipient)
                .Where(p => p.FileAnalysisId == fileAnalysisId)
                .ToListAsync();

            var detailModels = entities
                .Select(entity => ModelMapper.MapToDetailModel(entity))
                .Where(model => model != null)
                .ToList();

            return detailModels;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting Bitcoin packets by FileAnalysisId {FileAnalysisId}", fileAnalysisId);
            throw;
        }
    }
}
