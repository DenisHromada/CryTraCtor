namespace CryTraCtor.DataTypes;

public record KnownDomainDetail(
    string DomainName, // domain name that matches
    string Vendor, // e.g. Trezor, Ledger
    string Product, // e.g. trezor suite, ledger live
    string? Purpose, // e.g. blockbook, telemetry
    string? Cryptocurrency, // e.g. bitcoin, ethereum
    string? Description // additional information
);