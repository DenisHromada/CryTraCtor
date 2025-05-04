namespace CryTraCtor.Business.Models.Bitcoin;

public class BitcoinBlockHeaderModel
{
    public string BlockHash { get; set; } = string.Empty;
    public int Version { get; set; }
    public string PrevBlockHash { get; set; } = string.Empty;
    public string MerkleRoot { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public uint Bits { get; set; }
    public uint Nonce { get; set; }
}
