namespace CryTraCtor.Business.Models;

public class GenericPacketModel : IMessageModelBase
{
    public required Guid Id { get; set; }
    public Guid FileAnalysisId { get; set; }
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public DateTime Timestamp { get; set; }
}
