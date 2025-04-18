namespace CryTraCtor.Business.Models.CryptoProduct;

public class CryptoProductListModel : IModel
{
    public required Guid Id { get; set; }
    public required string Vendor { get; set; }
    public required string ProductName { get; set; }

    public static CryptoProductListModel Empty()
        => new CryptoProductListModel
        {
            Id = Guid.Empty,
            Vendor = string.Empty,
            ProductName = string.Empty
        };
}
