namespace CryTraCtor.Business.Models;

public record DnsPacketModel : IModel
{
    public required Guid Id { get; set; }

    public DateTime Timestamp { get; set; }
    public ushort TransactionId { get; set; }

    public string QueryName { get; set; } = string.Empty;
    public string QueryType { get; set; } = string.Empty;

    public bool IsQuery { get; set; }
    public string? ResponseAddresses { get; set; }

    public Guid FileAnalysisId { get; set; }
}
