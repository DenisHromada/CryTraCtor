namespace CryTraCtor.Models.CryptoProduct;

public class CryptoProductListModel : IModel
{
    public Guid Id { get; set; }
    public string Vendor { get; set; }
    public string ProductName { get; set; }
    
    public static CryptoProductListModel Empty()
        => new CryptoProductListModel
        {
            Id = Guid.Empty,
            Vendor = string.Empty,
            ProductName = string.Empty
        };
}