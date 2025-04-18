using CryTraCtor.Business.Models.KnownDomain;

namespace CryTraCtor.Business.Models.CryptoProduct;

public class CryptoProductDetailModel : IModel
{
    public required Guid Id { get; set; }
    public required string Vendor { get; set; }
    public required string ProductName { get; set; }
    public required ICollection<KnownDomainListModel> KnownDomains { get; set; }

    public static CryptoProductDetailModel Empty()
        => new()
        {
            Id = Guid.Empty,
            Vendor = string.Empty,
            ProductName = string.Empty,
            KnownDomains = []
        };
}
