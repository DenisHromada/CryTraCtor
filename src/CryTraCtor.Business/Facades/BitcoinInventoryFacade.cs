using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class BitcoinInventoryFacade(
    IUnitOfWorkFactory unitOfWorkFactory
) : IBitcoinInventoryFacade
{
    public async Task<BitcoinInventoryItemListModel?> GetModelByIdAsync(Guid inventoryId)
    {
        await using var uow = unitOfWorkFactory.Create();
        var inventoryRepository = uow.BitcoinInventories;

        var entity = await inventoryRepository.GetByIdAsync(inventoryId);

        if (entity == null)
        {
            return null;
        }

        return new BitcoinInventoryItemListModel
        {
            Id = entity.Id,
            Type = entity.Type,
            Hash = entity.Hash
        };
    }

    public async Task<BitcoinInventoryItemDetailModel?> GetDetailModelByIdAsync(Guid inventoryId, Guid fileAnalysisId)
    {
        await using var uow = unitOfWorkFactory.Create();
        var inventoryRepository = uow.BitcoinInventories;

        var inventoryEntity = await inventoryRepository.Get()
            .Where(inv => inv.Id == inventoryId)
            .Include(inv => inv.BitcoinPacketInventories)
            .ThenInclude(bpi => bpi.BitcoinPacket)
            .ThenInclude(packet => packet.Sender)
            .Include(inv => inv.BitcoinPacketInventories)
            .ThenInclude(bpi => bpi.BitcoinPacket)
            .ThenInclude(packet => packet.Recipient)
            .FirstOrDefaultAsync();

        if (inventoryEntity == null)
        {
            return null;
        }

        var detailModel = new BitcoinInventoryItemDetailModel
        {
            Id = inventoryEntity.Id,
            Type = inventoryEntity.Type,
            Hash = inventoryEntity.Hash,
            ReferencingPackets = inventoryEntity.BitcoinPacketInventories
                .Where(bpi => bpi.BitcoinPacket.FileAnalysisId == fileAnalysisId)
                .Select(bpi => new BitcoinPacketDetailModel
                {
                    Id = bpi.BitcoinPacket.Id,
                    FileAnalysisId = bpi.BitcoinPacket.FileAnalysisId,
                    SenderId = bpi.BitcoinPacket.SenderId,
                    RecipientId = bpi.BitcoinPacket.RecipientId,
                    Timestamp = bpi.BitcoinPacket.Timestamp,
                    Command = bpi.BitcoinPacket.Command,
                    Sender = bpi.BitcoinPacket.Sender != null
                        ? new TrafficParticipantListModel
                        {
                            Id = bpi.BitcoinPacket.Sender.Id,
                            Address = bpi.BitcoinPacket.Sender.Address,
                            Port = bpi.BitcoinPacket.Sender.Port,
                        }
                        : null,
                    Recipient = bpi.BitcoinPacket.Recipient != null
                        ? new TrafficParticipantListModel
                        {
                            Id = bpi.BitcoinPacket.Recipient.Id,
                            Address = bpi.BitcoinPacket.Recipient.Address,
                            Port = bpi.BitcoinPacket.Recipient.Port,
                        }
                        : null,
                })
                .ToList()
        };

        return detailModel;
    }
}
