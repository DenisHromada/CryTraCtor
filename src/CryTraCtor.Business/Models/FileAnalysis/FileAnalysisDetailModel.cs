using CryTraCtor.Business.Models.TrafficParticipants;

namespace CryTraCtor.Business.Models.FileAnalysis;

public class FileAnalysisDetailModel : IModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid StoredFileId { get; set; }

    public ICollection<TrafficParticipantListModel> TrafficParticipants { get; set; } = new List<TrafficParticipantListModel>();

    public static FileAnalysisDetailModel Empty() => new();
}
