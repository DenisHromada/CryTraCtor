namespace CryTraCtor.Business.Models.TrafficParticipants;

public class TrafficParticipantListModel : IModel
{
    public Guid Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }
    public Guid FileAnalysisId { get; set; }
    public int OutgoingDnsCount { get; set; }
    public int IncomingDnsCount { get; set; }
    public int UniqueMatchedKnownDomainCount { get; set; }
    public int TotalMatchedKnownDomainCount { get; set; }

    public static TrafficParticipantListModel Empty() => new();
}
