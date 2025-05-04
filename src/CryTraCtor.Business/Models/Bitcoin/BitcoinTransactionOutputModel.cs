namespace CryTraCtor.Business.Models.Bitcoin;

public class BitcoinTransactionOutputModel
{
    public long Value { get; set; }
    public string ScriptPubKey { get; set; } = string.Empty;
}
