using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.FileAnalysis;
using CryTraCtor.Business.Models.FileAnalysis;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class FileAnalysisFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    FileAnalysisModelMapper modelMapper
) : FacadeBase<
        FileAnalysisEntity,
        FileAnalysisListModel,
        FileAnalysisDetailModel,
        FileAnalysisEntityMapper>(unitOfWorkFactory, modelMapper),
    IFileAnalysisFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail => new List<string> { nameof(FileAnalysisEntity.TrafficParticipants) };

    public async Task<IEnumerable<FileAnalysisListModel>> GetByStoredFileIdAsync(Guid storedFileId)
    {
        await using var unitOfWork = UnitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<FileAnalysisEntity, FileAnalysisEntityMapper>();
        
        var entities = await repository.Get()
            .Where(fa => fa.StoredFileId == storedFileId)
            .ToListAsync();
            
        return ModelMapper.MapToListModel(entities);
    }
} 