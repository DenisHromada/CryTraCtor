using CryTraCtor.Analyzers;
using CryTraCtor.Mappers;

// Configure capture file
var captureFilePath = "";

// Extract DNS transactions
var domainNameDetector = new DnsTransactionExtractor(captureFilePath ?? string.Empty);
domainNameDetector.Run();
var dnsTransactions = domainNameDetector.DnsTransactions;
Console.WriteLine("Transaction count: {0}", dnsTransactions.Count);

var knownDomainDetector = new KnownDomainDetector(dnsTransactions);
knownDomainDetector.Run();
Console.WriteLine("Known domain count: {0}", knownDomainDetector.KnownDomainDetails.Count);

var walletIpAddresses = knownDomainDetector.GetKnownDomainIpAddresses();
foreach (var knownWalletKeyPair in walletIpAddresses)
{
    Console.WriteLine(knownWalletKeyPair.Key);
    Console.WriteLine(string.Join(", ", knownWalletKeyPair.Value));
}