namespace CryTraCtor.Database.Dtos;

public class DnsMessageFlatPurposeDto
{
    public required Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public ushort TransactionId { get; set; }
    public string QueryName { get; set; } = string.Empty;
    public string QueryType { get; set; } = string.Empty;
    public bool IsQuery { get; set; }
    public Guid FileAnalysisId { get; set; }

    public Guid SenderId { get; set; }
    public string? SenderAddress { get; set; }
    public int? SenderPort { get; set; }

    public Guid RecipientId { get; set; }
    public string? RecipientAddress { get; set; }
    public int? RecipientPort { get; set; }

    public string? Purpose { get; set; }
}
