using System.Configuration;
using CryTraCtor.TrafficAnalyzers;

var captureFilePath = ConfigurationManager.AppSettings["CaptureFilePath"];

var domainNameDetector = new DomainNameDetector(captureFilePath ?? string.Empty);
domainNameDetector.Run();
