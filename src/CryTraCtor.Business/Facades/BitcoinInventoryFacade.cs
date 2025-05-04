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
            .ThenInclude(bpi => bpi.BitcoinMessage)
            .ThenInclude(packet => packet.Sender)
            .Include(inv => inv.BitcoinPacketInventories)
            .ThenInclude(bpi => bpi.BitcoinMessage)
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
                .Where(bpi => bpi.BitcoinMessage.FileAnalysisId == fileAnalysisId)
                .Select(bpi => new BitcoinMessageDetailModel
                {
                    Id = bpi.BitcoinMessage.Id,
                    FileAnalysisId = bpi.BitcoinMessage.FileAnalysisId,
                    SenderId = bpi.BitcoinMessage.SenderId,
                    RecipientId = bpi.BitcoinMessage.RecipientId,
                    Timestamp = bpi.BitcoinMessage.Timestamp,
                    Command = bpi.BitcoinMessage.Command,
                    Sender = bpi.BitcoinMessage.Sender != null
                        ? new TrafficParticipantListModel
                        {
                            Id = bpi.BitcoinMessage.Sender.Id,
                            Address = bpi.BitcoinMessage.Sender.Address,
                            Port = bpi.BitcoinMessage.Sender.Port,
                        }
                        : null,
                    Recipient = bpi.BitcoinMessage.Recipient != null
                        ? new TrafficParticipantListModel
                        {
                            Id = bpi.BitcoinMessage.Recipient.Id,
                            Address = bpi.BitcoinMessage.Recipient.Address,
                            Port = bpi.BitcoinMessage.Recipient.Port,
                        }
                        : null,
                })
                .ToList()
        };

        return detailModel;
    }
}
