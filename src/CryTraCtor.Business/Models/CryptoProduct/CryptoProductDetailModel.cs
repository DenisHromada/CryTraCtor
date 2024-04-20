using CryTraCtor.Business.Models.KnownDomain;

namespace CryTraCtor.Business.Models.CryptoProduct;

public class CryptoProductDetailModel : IModel
{
    public Guid Id { get; set; }
    public string Vendor { get; set; }
    public string ProductName { get; set; }
    public ICollection<KnownDomainListModel> KnownDomains { get; set; }
    
    public static CryptoProductDetailModel Empty()
        => new CryptoProductDetailModel
        {
            Id = Guid.Empty,
            Vendor = string.Empty,
            ProductName = string.Empty,
        };
}