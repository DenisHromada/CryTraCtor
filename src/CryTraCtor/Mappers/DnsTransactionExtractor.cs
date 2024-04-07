using System.Collections.ObjectModel;
using CryTraCtor.Mappers.RawToSummaryMapper.Dns;
using CryTraCtor.Models.DnsTransaction;
using CryTraCtor.Models.Packet.Summary.Dns;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.Mappers;

public class DnsTransactionExtractor(string analyzedFileName) : TrafficExtractor(analyzedFileName)
{
    public Collection<DnsTransactionSummary> DnsTransactions { get; } = [];
    private readonly Dictionary<uint, DnsTransactionTraffic> _dnsTransactionDictionary = new();
    
    public override void Run()
    {
        using var device = new CaptureFileReaderDevice(AnalyzedFileName);

        device.Open();
        device.Filter = "ip and udp and port 53";

        device.OnPacketArrival += HandlePacketArrival;

        device.Capture();
        device.Close();
        PostProcessTransactions();
    }

    private void HandlePacketArrival(object s, PacketCapture packetCapture)
    {
        var dnsSummary = DnsMapper.MapPacketCaptureToDnsPacketSummary(packetCapture);

        AddPacketDnsSummaryToTransactions(dnsSummary);
    }

    private void AddPacketDnsSummaryToTransactions(IDnsSummary dnsSummary)
    {
        if (_dnsTransactionDictionary.TryGetValue(dnsSummary.TransactionId, out var value))
        {
            value.AddDnsSummary(dnsSummary);
        }
        else
        {
            var newDnsTransaction =
                new DnsTransactionTraffic(new Collection<DnsResponse>(), new Collection<DnsQuery>());
            newDnsTransaction.AddDnsSummary(dnsSummary);
            _dnsTransactionDictionary.Add(dnsSummary.TransactionId, newDnsTransaction);
        }
    }

    private void PostProcessTransactions()
    {
        foreach (var transactionDictPair in _dnsTransactionDictionary)
        {
            var dnsTransactionSummaries = DnsTrafficToTransaction.MapDnsTrafficToTransactions(transactionDictPair.Value);
            foreach (var transactionSummary in dnsTransactionSummaries)
            {
                DnsTransactions.Add(transactionSummary);
            }
        }
    }
    
    public static Collection<DnsTransactionSummary> MapDnsTrafficToTransactions(DnsTransactionTraffic dnsTraffic)
    {
        var dnsTransactions = new Collection<DnsTransactionSummary>();

        if (dnsTraffic.Queries.Count != 1 || dnsTraffic.Responses.Count != 1)
        {
            throw new NotImplementedException("Found queries/responses with identical DNS transaction id's.");
        }

        var query = dnsTraffic.Queries.First();
        var response = dnsTraffic.Responses.First();

        if (!QueryMatchesResponse(query, response))
        {
            throw new ApplicationException("Query does not match response.");
        }

        var transactionSummary = new DnsTransactionSummary(
            response.TransactionId,
            query.Source,
            query.Destination,
            query.Query,
            response.Answers
        );
        
        dnsTransactions.Add(transactionSummary);

        return dnsTransactions;
    }

    private static bool QueryMatchesResponse(DnsQuery query, DnsResponse response)
    {
        return (query.TransactionId == response.TransactionId
                && query.Source == response.Destination
                && query.Query == response.Query
            );
    }
}