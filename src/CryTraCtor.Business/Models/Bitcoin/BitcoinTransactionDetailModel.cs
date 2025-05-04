namespace CryTraCtor.Business.Models.Bitcoin;

public class BitcoinTransactionDetailModel
{
    public string TxId { get; set; } = string.Empty;
    public int Version { get; set; }
    public uint Locktime { get; set; }

    public List<BitcoinTransactionInputModel> Inputs { get; set; } = new List<BitcoinTransactionInputModel>();
    public List<BitcoinTransactionOutputModel> Outputs { get; set; } = new List<BitcoinTransactionOutputModel>();
}
