namespace CryTraCtor.Business.Models.Bitcoin;

public class BitcoinTransactionInputModel
{
    public string PreviousTxHash { get; set; } = string.Empty;
    public uint PreviousOutputIndex { get; set; }
    public string ScriptSig { get; set; } = string.Empty;
    public uint Sequence { get; set; }
}
