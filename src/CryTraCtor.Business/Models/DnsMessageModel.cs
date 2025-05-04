using CryTraCtor.Business.Models.TrafficParticipants;

namespace CryTraCtor.Business.Models;

public record DnsMessageModel : IMessageModelBase
{
    public required Guid Id { get; set; }

    public Guid FileAnalysisId { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid SenderId { get; set; }
    public TrafficParticipantListModel? Sender { get; set; }
    public Guid RecipientId { get; set; }
    public TrafficParticipantListModel? Recipient { get; set; }

    public ushort TransactionId { get; set; }
    public string QueryName { get; set; } = string.Empty;
    public string QueryType { get; set; } = string.Empty;
    public bool IsQuery { get; set; }
    public string? ResponseAddresses { get; set; }
    public string? KnownDomainPurpose { get; set; }
}
