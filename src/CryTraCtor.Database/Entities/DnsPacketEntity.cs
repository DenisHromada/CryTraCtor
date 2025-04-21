using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Index(nameof(FileAnalysisId))]
[Index(nameof(Timestamp))]
public class DnsPacketEntity : IEntity
{
    public required Guid Id { get; set; }

    public DateTime Timestamp { get; set; }
    public ushort TransactionId { get; set; }

    public string QueryName { get; set; } = string.Empty;
    public string QueryType { get; set; } = string.Empty;

    public bool IsQuery { get; set; }
    public string? ResponseAddresses { get; set; }

    public Guid FileAnalysisId { get; set; }
    public FileAnalysisEntity? FileAnalysis { get; set; }
}
