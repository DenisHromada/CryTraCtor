namespace CryTraCtor.Database.Entities;

public class KnownDomainEntity : IEntity
{
    public required Guid Id { get; set; }
    public required string DomainName { get; set; } // domain name that matches
    public required string Purpose { get; set; } // e.g. bitcoin blockbook, telemetry
    public required string Description { get; set; } // additional information

    public required Guid CryptoProductId { get; set; }
    public CryptoProductEntity? CryptoProduct { get; set; }
}
