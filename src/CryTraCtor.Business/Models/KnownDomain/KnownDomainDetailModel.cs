using CryTraCtor.Models.CryptoProduct;

namespace CryTraCtor.Models.KnownDomain;

public class KnownDomainDetailModel : IModel
{
    public Guid Id { get; set; }
    public string DomainName { get; set; }
    public string Purpose { get; set; }
    public string Description { get; set; }
    public CryptoProductListModel CryptoProduct { get; set; } 
    
    public static KnownDomainDetailModel Empty()
        => new KnownDomainDetailModel
        {
            Id = Guid.Empty,
            DomainName = string.Empty,
            Purpose = string.Empty,
            Description = string.Empty,
            CryptoProduct = CryptoProductListModel.Empty()
        };
}