using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.Bitcoin;
using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Facades;

public class BitcoinPacketFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    BitcoinPacketModelMapper modelMapper,
    ILogger<BitcoinPacketFacade> logger)
    : FacadeBase<BitcoinPacketEntity, BitcoinPacketListModel, BitcoinPacketDetailModel, BitcoinPacketEntityMapper>(
        unitOfWorkFactory, modelMapper), IBitcoinPacketFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new List<string>
        {
            nameof(BitcoinPacketEntity.Sender),
            nameof(BitcoinPacketEntity.Recipient),
            $"{nameof(BitcoinPacketEntity.BitcoinPacketInventories)}.{nameof(BitcoinPacketInventoryEntity.BitcoinInventory)}",
            $"{nameof(BitcoinPacketEntity.BitcoinPacketTransactions)}.{nameof(BitcoinPacketTransactionEntity.BitcoinTransaction)}.{nameof(BitcoinTransactionEntity.Inputs)}",
            $"{nameof(BitcoinPacketEntity.BitcoinPacketTransactions)}.{nameof(BitcoinPacketTransactionEntity.BitcoinTransaction)}.{nameof(BitcoinTransactionEntity.Outputs)}",
            $"{nameof(BitcoinPacketEntity.BitcoinPacketHeaders)}.{nameof(BitcoinPacketHeaderEntity.BitcoinBlockHeader)}"
        };

    public override async Task<BitcoinPacketDetailModel> CreateOrUpdateAsync(BitcoinPacketDetailModel model)
    {
        try
        {
            await using var uow = UnitOfWorkFactory.Create();
            var packetRepository = uow.GetRepository<BitcoinPacketEntity, BitcoinPacketEntityMapper>();
            var inventoryRepository = uow.BitcoinInventories;
            var transactionRepository = uow.BitcoinTransactions;
            var blockHeaderRepository = uow.BitcoinBlockHeaders;


            var entity = ModelMapper.MapToEntity(model);

            switch (model.Command)
            {
                case "inv" or "getdata" or "notfound" when model.Inventories != null && model.Inventories.Count != 0:
                {
                    foreach (var invItemModel in model.Inventories)
                    {
                        var inventoryEntity =
                            await inventoryRepository.GetOrCreateAsync(invItemModel.Type, invItemModel.Hash);

                        var packetInventoryEntity = new BitcoinPacketInventoryEntity
                        {
                            BitcoinPacket = entity,
                            BitcoinInventory = inventoryEntity
                        };

                        entity.BitcoinPacketInventories.Add(packetInventoryEntity);
                    }

                    break;
                }
                case "tx" when model.Transaction != null:
                {
                    var transactionEntity =
                        await transactionRepository.GetByTxIdAsync(model.Transaction.TxId);

                    if (transactionEntity == null)
                    {
                        transactionEntity = new BitcoinTransactionEntity
                        {
                            TxId = model.Transaction.TxId,
                            Version = model.Transaction.Version,
                            Locktime = model.Transaction.Locktime,
                            Inputs = model.Transaction.Inputs
                                .Select(inputModel => new BitcoinTransactionInputEntity
                                {
                                    PreviousTxHash = inputModel.PreviousTxHash,
                                    PreviousOutputIndex = inputModel.PreviousOutputIndex,
                                    ScriptSig = inputModel.ScriptSig,
                                    Sequence = inputModel.Sequence
                                })
                                .ToList(),
                            Outputs = model.Transaction.Outputs
                                .Select(outputModel => new BitcoinTransactionOutputEntity
                                {
                                    Value = outputModel.Value,
                                    ScriptPubKey = outputModel.ScriptPubKey
                                })
                                .ToList()
                        };
                        await transactionRepository.InsertAsync(transactionEntity);
                    }

                    var packetTransactionEntity = new BitcoinPacketTransactionEntity
                    {
                        BitcoinPacket = entity,
                        BitcoinTransaction = transactionEntity
                    };

                    entity.BitcoinPacketTransactions.Add(packetTransactionEntity);
                    break;
                }
                case "headers" when model.Headers != null && model.Headers.Count != 0:
                {
                    foreach (var headerModel in model.Headers)
                    {
                        var blockHeaderEntity =
                            await blockHeaderRepository.GetOrCreateByBlockHashAsync(headerModel.BlockHash,
                                new BitcoinBlockHeaderEntity
                                {
                                    BlockHash = headerModel.BlockHash,
                                    Version = headerModel.Version,
                                    PrevBlockHash = headerModel.PrevBlockHash,
                                    MerkleRoot = headerModel.MerkleRoot,
                                    Timestamp = headerModel.Timestamp,
                                    Bits = headerModel.Bits,
                                    Nonce = headerModel.Nonce
                                });

                        var packetHeaderEntity = new BitcoinPacketHeaderEntity
                        {
                            BitcoinPacket = entity,
                            BitcoinBlockHeader = blockHeaderEntity
                        };

                        entity.BitcoinPacketHeaders.Add(packetHeaderEntity);
                    }

                    break;
                }
            }


            await packetRepository.InsertAsync(entity);
            await uow.CommitAsync();


            return ModelMapper.MapToDetailModel(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating or updating Bitcoin packet with Command {Command}", model.Command);
            throw;
        }
    }


    public async Task<IEnumerable<BitcoinPacketDetailModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        try
        {
            await using var uow = UnitOfWorkFactory.Create();

            var repository = uow.GetRepository<BitcoinPacketEntity, BitcoinPacketEntityMapper>();

            var entities = await repository.Get()
                .Include(p => p.Sender)
                .Include(p => p.Recipient)
                .Include(p => p.BitcoinPacketInventories)
                .Include(p => p.BitcoinPacketTransactions)
                .ThenInclude(ppt => ppt.BitcoinTransaction)
                .ThenInclude(t => t.Inputs)
                .Include(p => p.BitcoinPacketTransactions)
                .ThenInclude(ppt => ppt.BitcoinTransaction)
                .ThenInclude(t => t.Outputs)
                .Include(p => p.BitcoinPacketHeaders)
                .ThenInclude(pph => pph.BitcoinBlockHeader)
                .Where(p => p.FileAnalysisId == fileAnalysisId)
                .ToListAsync();

            var detailModels = entities
                .Select(entity => ModelMapper.MapToDetailModel(entity))
                .ToList();

            return detailModels;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting Bitcoin packets by FileAnalysisId {FileAnalysisId}", fileAnalysisId);
            throw;
        }
    }

    public async Task<IEnumerable<BitcoinPacketListModel>> GetPacketListByInventoryIdAndAnalysisIdAsync(
        Guid inventoryId, Guid fileAnalysisId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.BitcoinPackets;

        var entities = await repository.GetByInventoryIdAndAnalysisIdAsync(inventoryId, fileAnalysisId);

        return entities.Select(entity => ModelMapper.MapToListModel(entity)).ToList();
    }
}
