namespace CryTraCtor.Business.Models.KnownDomain;

public class KnownDomainImportModel
{
    public string Vendor { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;

    public string DomainName { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public static KnownDomainImportModel Empty()
        => new KnownDomainImportModel
        {
            Vendor = string.Empty,
            ProductName = string.Empty,
            DomainName = string.Empty,
            Purpose = string.Empty,
            Description = string.Empty
        };
}
