namespace CryTraCtor.Database.Entities;

public class CryptoProductEntity : IEntity
{
    public required Guid Id { get; set; }
    public string Vendor { get; set; } // e.g. Trezor, Ledger
    public string ProductName { get; set; } // e.g. trezor suite, ledger live

    public ICollection<KnownDomainEntity> KnownDomains { get; init; } = new List<KnownDomainEntity>();
}