using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class DnsPacketFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    DnsPacketModelMapper modelMapper)
    : FacadeBase<DnsPacketEntity, DnsPacketModel, DnsPacketModel, DnsPacketEntityMapper>(unitOfWorkFactory,
        modelMapper), IDnsPacketFacade
{
    public async Task<IEnumerable<DnsPacketModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        // Use generic repository from UoW and build query here
        var repository = uow.GetRepository<DnsPacketEntity, DnsPacketEntityMapper>();

        var entities = await repository.Get()
            .Include(e => e.Sender)
            .Include(e => e.Recipient)
            .Where(e => e.FileAnalysisId == fileAnalysisId)
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }
}
