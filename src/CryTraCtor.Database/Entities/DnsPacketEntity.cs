using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Index(nameof(FileAnalysisId))]
[Index(nameof(Timestamp))]
[Index(nameof(SenderId))]
[Index(nameof(RecipientId))]
public class DnsPacketEntity : IEntity
{
    public required Guid Id { get; set; }

    [ForeignKey(nameof(Sender))]
    public Guid SenderId { get; set; }
    public TrafficParticipantEntity? Sender { get; set; }

    [ForeignKey(nameof(Recipient))]
    public Guid RecipientId { get; set; }
    public TrafficParticipantEntity? Recipient { get; set; }

    public DateTime Timestamp { get; set; }
    public ushort TransactionId { get; set; }

    public string QueryName { get; set; } = string.Empty;
    public string QueryType { get; set; } = string.Empty;

    public bool IsQuery { get; set; }
    public string? ResponseAddresses { get; set; }

    public Guid FileAnalysisId { get; set; }
    public FileAnalysisEntity? FileAnalysis { get; set; }
}
