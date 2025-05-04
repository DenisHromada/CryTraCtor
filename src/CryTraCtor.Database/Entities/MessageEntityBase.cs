using System.ComponentModel.DataAnnotations.Schema;

namespace CryTraCtor.Database.Entities;

public abstract class MessageEntityBase : IEntity
{
    public required Guid Id { get; set; }

    [ForeignKey(nameof(FileAnalysisId))]
    public Guid FileAnalysisId { get; set; }
    public FileAnalysisEntity? FileAnalysis { get; set; }

    [ForeignKey(nameof(SenderId))]
    public Guid SenderId { get; set; }
    public TrafficParticipantEntity? Sender { get; set; }

    public Guid RecipientId { get; set; }
    [ForeignKey(nameof(RecipientId))] public TrafficParticipantEntity? Recipient { get; set; }

    public DateTime Timestamp { get; set; }
}
