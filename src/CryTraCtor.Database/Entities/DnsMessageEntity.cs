using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Index(nameof(FileAnalysisId))]
[Index(nameof(Timestamp))]
[Index(nameof(SenderId))]
[Index(nameof(RecipientId))]
public class DnsMessageEntity : MessageEntityBase
{
    public ushort TransactionId { get; set; }
    public string QueryName { get; set; } = string.Empty;
    public string QueryType { get; set; } = string.Empty;
    public bool IsQuery { get; set; }
    public string? ResponseAddresses { get; set; }
}
