using CryTraCtor.Business.Models.FileAnalysis;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IFileAnalysisFacade : IFacade<FileAnalysisEntity, FileAnalysisListModel, FileAnalysisDetailModel>
{
    Task<IEnumerable<FileAnalysisListModel>> GetByStoredFileIdAsync(Guid storedFileId);
} 