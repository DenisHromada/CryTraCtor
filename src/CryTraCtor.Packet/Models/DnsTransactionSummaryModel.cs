using System.Collections.ObjectModel;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

namespace CryTraCtor.Packet.Models;

public record DnsTransactionSummaryModel(
    uint TransactionId,
    InternetEndpointModel Client,
    InternetEndpointModel Server,
    DnsResourceRecordModel Query,
    Collection<DnsResourceRecordModel> Answers)
{
    public DnsTransactionSummaryModel(uint transactionId, InternetEndpointModel client, InternetEndpointModel server,
        DnsResourceRecordModel query)
        : this(transactionId, client, server, query, []
        )
    {
    }

    public uint TransactionId { get; set; } = TransactionId;
    public InternetEndpointModel Client { get; set; } = Client;
    public InternetEndpointModel Server { get; set; } = Server;
    public DnsResourceRecordModel Query { get; set; } = Query;
    public Collection<DnsResourceRecordModel> Answers { get; set; } = Answers;
}