using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.FileAnalysis;

namespace CryTraCtor.Business.Services;

public class FileAnalysisService(
    IFileAnalysisFacade fileAnalysisFacade,
    IStoredFileFacade storedFileFacade,
    EndpointAnalysisService endpointAnalysisService,
    DnsAnalysisService dnsAnalysisService
)
{
    public async Task<FileAnalysisDetailModel> CreateAnalysis(Guid storedFileId)
    {
        var storedFile = await storedFileFacade.GetByIdAsync(storedFileId);
        if (storedFile == null)
        {
            throw new ArgumentException($"Stored file with ID {storedFileId} not found.", nameof(storedFileId));
        }

        var fileAnalysisModel = new FileAnalysisDetailModel
        {
            Id = Guid.NewGuid(),
            Name = $"Analysis - {storedFile.PublicFileName}",
            CreatedAt = DateTime.UtcNow,
            StoredFileId = storedFileId
        };
        var createdAnalysis = await fileAnalysisFacade.CreateOrUpdateAsync(fileAnalysisModel);


        await endpointAnalysisService.AnalyzeAsync(storedFile, createdAnalysis.Id);

        await dnsAnalysisService.AnalyzeAsync(storedFile, createdAnalysis.Id);

        return createdAnalysis;
    }
}
