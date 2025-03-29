using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class TrafficParticipantFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    TrafficParticipantModelMapper modelMapper
) : FacadeBase<
        TrafficParticipantEntity,
        TrafficParticipantListModel,
        TrafficParticipantDetailModel,
        TrafficParticipantEntityMapper>(unitOfWorkFactory, modelMapper),
    ITrafficParticipantFacade
{
    public async Task<IEnumerable<TrafficParticipantListModel>> GetByStoredFileIdAsync(Guid storedFileId)
    {
        await using var unitOfWork = UnitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<TrafficParticipantEntity, TrafficParticipantEntityMapper>();
        
        var entities = await repository.Get()
            .Where(tp => tp.StoredFileId == storedFileId)
            .ToListAsync();
            
        return ModelMapper.MapToListModel(entities);
    }
} 