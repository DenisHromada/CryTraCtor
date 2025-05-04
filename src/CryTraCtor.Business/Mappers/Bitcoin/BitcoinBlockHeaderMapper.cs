using CryTraCtor.Business.Models.Bitcoin;
using NBitcoin;

namespace CryTraCtor.Business.Mappers.Bitcoin;

public class BitcoinBlockHeaderMapper
{
    public BitcoinBlockHeaderModel Map(BlockHeader nbitcoinHeader)
    {
        return new BitcoinBlockHeaderModel
        {
            BlockHash = nbitcoinHeader.GetHash().ToString(),
            Version = nbitcoinHeader.Version,
            PrevBlockHash = nbitcoinHeader.HashPrevBlock.ToString(),
            MerkleRoot = nbitcoinHeader.HashMerkleRoot.ToString(),
            Timestamp = nbitcoinHeader.BlockTime,
            Bits = nbitcoinHeader.Bits,
            Nonce = nbitcoinHeader.Nonce
        };
    }
}
