using System.Collections.ObjectModel;
using CryTraCtor.Packet.DataTypeMappers;
using CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Dns;
using CryTraCtor.Packet.DataTypes.DnsTransaction;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Models;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.Packet.Services;

public class DnsTransactionExtractor : IDnsTransactionExtractor
{
    private Collection<DnsTransactionSummaryModel> DnsTransactions { get; set; } = [];
    private Dictionary<uint, DnsTransactionTraffic> _dnsTransactionDictionary = new();

    public Collection<DnsTransactionSummaryModel> Run(string fileName)
    {
        using var device = new CaptureFileReaderDevice(fileName);

        device.Open();
        device.Filter = "ip and udp and port 53";

        device.OnPacketArrival += HandlePacketArrival;

        device.Capture();
        device.Close();
        PostProcessTransactions();

        return DnsTransactions;
    }

    private void PostProcessTransactions()
    {
        foreach (var transactionDictPair in _dnsTransactionDictionary)
        {
            var dnsTransactionSummaries =
                DnsTrafficToTransaction.MapDnsTrafficToTransactions(transactionDictPair.Value);
            foreach (var transactionSummary in dnsTransactionSummaries)
            {
                DnsTransactions.Add(transactionSummary);
            }
        }
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
}