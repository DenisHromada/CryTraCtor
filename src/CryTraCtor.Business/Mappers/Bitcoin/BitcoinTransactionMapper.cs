using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.Bitcoin;

public class BitcoinTransactionMapper : ModelMapperBase<BitcoinTransactionEntity, BitcoinTransactionDetailModel,
    BitcoinTransactionDetailModel>
{
    public override BitcoinTransactionEntity MapToEntity(BitcoinTransactionDetailModel model)
    {
        if (model == null)
        {
            return null!;
        }

        return new BitcoinTransactionEntity
        {
            TxId = model.TxId,
            Version = model.Version,
            Locktime = model.Locktime,
        };
    }

    public override BitcoinTransactionDetailModel? MapToListModel(BitcoinTransactionEntity? entity)
    {
        return MapToDetailModel(entity);
    }

    public override BitcoinTransactionDetailModel? MapToDetailModel(BitcoinTransactionEntity? entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new BitcoinTransactionDetailModel
        {
            TxId = entity.TxId,
            Version = entity.Version,
            Locktime = entity.Locktime,
            Inputs = entity.Inputs?
                .Select(inputEntity => new BitcoinTransactionInputModel
                {
                    PreviousTxHash = inputEntity.PreviousTxHash,
                    PreviousOutputIndex = inputEntity.PreviousOutputIndex,
                    ScriptSig = inputEntity.ScriptSig,
                    Sequence = inputEntity.Sequence
                })
                .ToList() ?? new List<BitcoinTransactionInputModel>(),
            Outputs = entity.Outputs?
                .Select(outputEntity => new BitcoinTransactionOutputModel
                {
                    Value = outputEntity.Value,
                    ScriptPubKey = outputEntity.ScriptPubKey
                })
                .ToList() ?? new List<BitcoinTransactionOutputModel>()
        };
    }

    public BitcoinTransactionDetailModel Map(NBitcoin.Transaction nbitcoinTx)
    {
        if (nbitcoinTx == null)
        {
            return null!;
        }

        return new BitcoinTransactionDetailModel
        {
            TxId = nbitcoinTx.GetHash().ToString(),
            Version = (int)nbitcoinTx.Version,
            Locktime = nbitcoinTx.LockTime.Value,
            Inputs = nbitcoinTx.Inputs?
                .Select(txIn => new BitcoinTransactionInputModel
                {
                    PreviousTxHash = txIn.PrevOut.Hash.ToString(),
                    PreviousOutputIndex = txIn.PrevOut.N,
                    ScriptSig = txIn.ScriptSig.ToString(),
                    Sequence = txIn.Sequence.Value
                })
                .ToList() ?? new List<BitcoinTransactionInputModel>(),
            Outputs = nbitcoinTx.Outputs?
                .Select(txOut => new BitcoinTransactionOutputModel
                {
                    Value = txOut.Value.Satoshi,
                    ScriptPubKey = txOut.ScriptPubKey.ToString()
                })
                .ToList() ?? new List<BitcoinTransactionOutputModel>()
        };
    }
}
