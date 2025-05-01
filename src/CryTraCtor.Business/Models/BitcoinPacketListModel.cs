namespace CryTraCtor.Business.Models;

public class BitcoinPacketListModel : IPacketModelBase
{
    public Guid Id { get; set; }

    public Guid FileAnalysisId { get; set; }
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Command { get; set; } = string.Empty;
}
