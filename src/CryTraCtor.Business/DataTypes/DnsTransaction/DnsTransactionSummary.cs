using System.Collections.ObjectModel;
using CryTraCtor.DataTypes.Packet.Summary.Dns;

namespace CryTraCtor.DataTypes.DnsTransaction;

public record DnsTransactionSummary(
    uint TransactionId,
    InternetEndpoint Client,
    InternetEndpoint Server,
    DnsResourceRecord Query,
    Collection<DnsResourceRecord> Answers)
{
    public DnsTransactionSummary(uint transactionId, InternetEndpoint client, InternetEndpoint server,
        DnsResourceRecord query)
        : this(transactionId, client, server, query, []
        )
    {
    }

    public uint TransactionId { get; set; } = TransactionId;
    public InternetEndpoint Client { get; set; } = Client;
    public InternetEndpoint Server { get; set; } = Server;
    public DnsResourceRecord Query { get; set; } = Query;
    public Collection<DnsResourceRecord> Answers { get; set; } = Answers;
}