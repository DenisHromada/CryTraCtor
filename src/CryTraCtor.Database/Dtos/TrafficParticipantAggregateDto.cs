namespace CryTraCtor.Database.Dtos;

public class TrafficParticipantAggregateDto
{
    public required Guid Id { get; set; }
    public required string Address { get; set; }
    public required int Port { get; set; }
    public required Guid FileAnalysisId { get; set; }
    public int OutgoingDnsCount { get; set; }
    public int IncomingDnsCount { get; set; }
    public int OutgoingBitcoinCount { get; set; }
    public int IncomingBitcoinCount { get; set; }
    public int UniqueMatchedKnownDomainCount { get; set; }
    public int TotalMatchedKnownDomainCount { get; set; }
}
