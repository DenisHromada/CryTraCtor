using CryTraCtor.Business.Models.CryptoProduct;

namespace CryTraCtor.Business.Models.KnownDomain;

public class KnownDomainDetailModel : IModel
{
    public Guid Id { get; set; }
    public string DomainName { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public CryptoProductListModel CryptoProduct { get; set; } = CryptoProductListModel.Empty();

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
