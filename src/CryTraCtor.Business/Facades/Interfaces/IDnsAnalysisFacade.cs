namespace CryTraCtor.Business.Facades.Interfaces;

public interface IDnsAnalysisFacade
{
    Task AnalyzeDnsPacketsAsync(Guid fileAnalysisId, Guid storedFileId);
}
