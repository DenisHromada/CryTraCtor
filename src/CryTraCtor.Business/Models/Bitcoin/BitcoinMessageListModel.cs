namespace CryTraCtor.Business.Models.Bitcoin;

public class BitcoinMessageListModel : IMessageModelBase
{
    public Guid Id { get; set; }

    public Guid FileAnalysisId { get; set; }
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Command { get; set; } = string.Empty;
    public int? InventoryCount { get; set; }
}
