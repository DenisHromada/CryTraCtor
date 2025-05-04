namespace CryTraCtor.Business.Models.Bitcoin;

public class BitcoinInventoryItemListModel : IModel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
}
