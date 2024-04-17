namespace CryTraCtor.Database.Entities;

public record KnownProduct()
{
    string Vendor { get; init; } // e.g. Trezor, Ledger
    string ProductName { get; init; } // e.g. trezor suite, ledger live
}