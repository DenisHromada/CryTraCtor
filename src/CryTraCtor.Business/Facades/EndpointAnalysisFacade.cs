using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.FileAnalysis;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Services;
using CryTraCtor.Packet.Services;

namespace CryTraCtor.Business.Facades;
public class EndpointAnalysisFacade(
    IStoredFileFacade storedFileFacade,
    IEndpointReader endpointReader,
    IFileStorageService fileStorageService,
    IFileAnalysisFacade fileAnalysisFacade,
    ITrafficParticipantFacade trafficParticipantFacade)
    : IEndpointAnalysisFacade
{
    public async Task<Guid> AnalyzeEndpointsAsync(Guid storedFileId)
    {
        var storedFile = await storedFileFacade.GetByIdAsync(storedFileId);

        if (storedFile == null)
        {
            throw new ArgumentException($"Stored file with ID {storedFileId} not found.", nameof(storedFileId));
        }

        var filePath = Path.Combine(fileStorageService.FileStorageDirectory, storedFile.InternalFilePath);
        if (string.IsNullOrEmpty(storedFile.InternalFilePath) || !File.Exists(filePath))
        {
            throw new InvalidOperationException(
                $"Pcap file path not found or invalid for stored file ID {storedFileId}. Constructed Path: {filePath}");
        }

        var endpoints = endpointReader.GetEndpoints(filePath);

        var fileAnalysisModel = new FileAnalysisDetailModel
        {
            Id = Guid.NewGuid(),
            Name = $"Endpoint Analysis - {storedFile.PublicFileName}",
            CreatedAt = DateTime.UtcNow,
            StoredFileId = storedFileId
        };
        var createdAnalysis = await fileAnalysisFacade.CreateOrUpdateAsync(fileAnalysisModel);

        foreach (var endpoint in endpoints)
        {
            var participantModel = new TrafficParticipantDetailModel
            {
                Id = Guid.NewGuid(),
                Address = endpoint.IpAddress.ToString(),
                Port = endpoint.Port,
                FileAnalysis = new FileAnalysisListModel { Id = createdAnalysis.Id }
            };
            await trafficParticipantFacade.CreateOrUpdateAsync(participantModel);
        }

        return createdAnalysis.Id;
    }
}
