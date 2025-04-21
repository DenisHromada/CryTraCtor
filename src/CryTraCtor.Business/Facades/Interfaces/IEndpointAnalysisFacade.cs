namespace CryTraCtor.Business.Facades.Interfaces;


public interface IEndpointAnalysisFacade
{
    Task<Guid> AnalyzeEndpointsAsync(Guid storedFileId);
}
