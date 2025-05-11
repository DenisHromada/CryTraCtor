using CryTraCtor.Business.Models.CryptoProduct;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Business.Models.Aggregates;

public class GroupedQueriedDomains(
    CryptoProductListModel groupKey,
    ICollection<DnsTransactionSummaryModel> toList
)
{
    public CryptoProductListModel CryptoProduct { get; set; } = groupKey;
    public ICollection<DnsTransactionSummaryModel> DnsTransactions { get; set; } = toList;
}
