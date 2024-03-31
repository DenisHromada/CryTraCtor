using System.Configuration;
using CryTraCtor.Analyzers;
using CryTraCtor.Packet;
using CryTraCtor.PacketParsers;

var captureFilePath = ConfigurationManager.AppSettings["CaptureFilePath"];

var domainNameDetector = new DnsTransactionExtractor(captureFilePath ?? string.Empty);
domainNameDetector.Run();
var dnsTransactions = domainNameDetector.DnsTransactions;
Console.WriteLine("Transaction count: {0}", dnsTransactions.Count);

var knownDomainDetector = new KnownDomainDetector(dnsTransactions);
knownDomainDetector.Run();

