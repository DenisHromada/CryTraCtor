using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.Bitcoin;

public class BitcoinPacketModelMapper(
    TrafficParticipantListModelMapper trafficParticipantMapper,
    BitcoinTransactionMapper bitcoinTransactionMapper
) : ModelMapperBase<BitcoinPacketEntity, BitcoinPacketListModel, BitcoinPacketDetailModel>
{
    public override BitcoinPacketEntity MapToEntity(BitcoinPacketDetailModel model)
        => new()
        {
            Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
            FileAnalysisId = model.FileAnalysisId,
            SenderId = model.SenderId,
            RecipientId = model.RecipientId,
            Timestamp = model.Timestamp,
            Magic = model.Magic,
            Command = model.Command,
            Length = model.Length,
            Checksum = model.Checksum
        };

    public override BitcoinPacketListModel MapToListModel(BitcoinPacketEntity? entity)
    {
        if (entity == null)
        {
            return null!;
        }

        return new BitcoinPacketListModel
        {
            Id = entity.Id,
            FileAnalysisId = entity.FileAnalysisId,
            SenderId = entity.SenderId,
            RecipientId = entity.RecipientId,
            Timestamp = entity.Timestamp,
            Command = entity.Command,
            InventoryCount = entity.Command is "inv" or "getdata" or "notfound"
                ? entity.BitcoinPacketInventories.Count
                : null
        };
    }

    public override BitcoinPacketDetailModel MapToDetailModel(BitcoinPacketEntity? entity)
    {
        if (entity == null)
        {
            return null!;
        }

        var detailModel = new BitcoinPacketDetailModel
        {
            Id = entity.Id,
            FileAnalysisId = entity.FileAnalysisId,
            SenderId = entity.SenderId,
            RecipientId = entity.RecipientId,
            Timestamp = entity.Timestamp,
            Magic = entity.Magic,
            Command = entity.Command,
            Length = entity.Length,
            Checksum = entity.Checksum,
            Sender = trafficParticipantMapper.MapToListModel(entity.Sender),
            Recipient = trafficParticipantMapper.MapToListModel(entity.Recipient),
            InventoryCount = entity.Command is "inv" or "getdata" or "notfound"
                ? entity.BitcoinPacketInventories.Count
                : null
        };

        switch (entity.Command)
        {
            case "inv" or "getdata" or "notfound":
            {
                detailModel.Inventories = entity.BitcoinPacketInventories
                    .Where(joinEntity => joinEntity?.BitcoinInventory != null)
                    .Select(joinEntity => new BitcoinInventoryItemModel
                    {
                        Id = joinEntity.BitcoinInventory.Id,
                        Type = joinEntity.BitcoinInventory.Type,
                        Hash = joinEntity.BitcoinInventory.Hash
                    })
                    .ToList() ?? [];
                break;
            }
            case "headers":
            {
                detailModel.Headers = entity.BitcoinPacketHeaders
                    .Where(joinEntity => joinEntity?.BitcoinBlockHeader != null)
                    .Select(joinEntity => new BitcoinBlockHeaderModel
                    {
                        BlockHash = joinEntity.BitcoinBlockHeader.BlockHash,
                        Version = joinEntity.BitcoinBlockHeader.Version,
                        PrevBlockHash = joinEntity.BitcoinBlockHeader.PrevBlockHash,
                        MerkleRoot = joinEntity.BitcoinBlockHeader.MerkleRoot,
                        Timestamp = joinEntity.BitcoinBlockHeader.Timestamp,
                        Bits = joinEntity.BitcoinBlockHeader.Bits,
                        Nonce = joinEntity.BitcoinBlockHeader.Nonce
                    })
                    .ToList() ?? [];
                break;
            }
            case "tx":
            {
                var transactionEntity = entity.BitcoinPacketTransactions.FirstOrDefault()?.BitcoinTransaction;
                if (transactionEntity != null)
                {
                    detailModel.Transaction = bitcoinTransactionMapper.MapToDetailModel(transactionEntity);
                }

                break;
            }
        }

        return detailModel;
    }
}
