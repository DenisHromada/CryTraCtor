using System.Configuration;
using CryTraCtor.TrafficAnalyzers;

var captureFilePath = ConfigurationManager.AppSettings["CaptureFilePath"];

var domainNameDetector = new DomainNameDetector(captureFilePath ?? string.Empty);
domainNameDetector.Run();
var dnsTransactions = domainNameDetector.DnsTransactions;
Console.WriteLine("Transaction count: {0}", dnsTransactions.Count);
