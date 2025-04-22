using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.FileAnalysis;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Packet.Services;

namespace CryTraCtor.Business.Services;

public class EndpointAnalysisService(
    IEndpointReader endpointReader,
    ITrafficParticipantFacade trafficParticipantFacade)
{
    public async Task AnalyzeAsync(StoredFileDetailModel storedFile, Guid fileAnalysisId, CancellationToken cancellationToken = default)
    {
        var endpoints = endpointReader.GetEndpoints(storedFile.InternalFilePath);

        foreach (var endpoint in endpoints)
        {
            var participantModel = new TrafficParticipantDetailModel
            {
                Id = Guid.NewGuid(),
                Address = endpoint.IpAddress.ToString(),
                Port = endpoint.Port,
                FileAnalysis = new FileAnalysisListModel { Id = fileAnalysisId }
            };
            await trafficParticipantFacade.CreateOrUpdateAsync(participantModel);
        }
    }
}
