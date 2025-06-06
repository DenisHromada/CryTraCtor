using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.Bitcoin;
using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Facades;

public class BitcoinMessageFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    BitcoinMessageModelMapper modelMapper,
    ILogger<BitcoinMessageFacade> logger)
    : FacadeBase<BitcoinMessageEntity, BitcoinMessageListModel, BitcoinMessageDetailModel, BitcoinMessageEntityMapper>(
        unitOfWorkFactory, modelMapper), IBitcoinMessageFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new List<string>
        {
            nameof(BitcoinMessageEntity.Sender),
            nameof(BitcoinMessageEntity.Recipient),
            $"{nameof(BitcoinMessageEntity.BitcoinMessageInventories)}.{nameof(BitcoinMessageInventoryEntity.BitcoinInventory)}",
            $"{nameof(BitcoinMessageEntity.BitcoinMessageTransactions)}.{nameof(BitcoinMessageTransactionEntity.BitcoinTransaction)}.{nameof(BitcoinTransactionEntity.Inputs)}",
            $"{nameof(BitcoinMessageEntity.BitcoinMessageTransactions)}.{nameof(BitcoinMessageTransactionEntity.BitcoinTransaction)}.{nameof(BitcoinTransactionEntity.Outputs)}",
            $"{nameof(BitcoinMessageEntity.BitcoinMessageHeaders)}.{nameof(BitcoinMessageHeaderEntity.BitcoinBlockHeader)}"
        };

    public override async Task<BitcoinMessageDetailModel> CreateOrUpdateAsync(BitcoinMessageDetailModel model)
    {
        try
        {
            await using var uow = UnitOfWorkFactory.Create();
            var packetRepository = uow.GetRepository<BitcoinMessageEntity, BitcoinMessageEntityMapper>();
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

                        var packetInventoryEntity = new BitcoinMessageInventoryEntity
                        {
                            BitcoinMessage = entity,
                            BitcoinInventory = inventoryEntity
                        };

                        entity.BitcoinMessageInventories.Add(packetInventoryEntity);
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

                    var packetTransactionEntity = new BitcoinMessageTransactionEntity
                    {
                        BitcoinMessage = entity,
                        BitcoinTransaction = transactionEntity
                    };

                    entity.BitcoinMessageTransactions.Add(packetTransactionEntity);
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

                        var packetHeaderEntity = new BitcoinMessageHeaderEntity
                        {
                            BitcoinMessage = entity,
                            BitcoinBlockHeader = blockHeaderEntity
                        };

                        entity.BitcoinMessageHeaders.Add(packetHeaderEntity);
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


    public async Task<IEnumerable<BitcoinMessageDetailModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        try
        {
            await using var uow = UnitOfWorkFactory.Create();

            var repository = uow.GetRepository<BitcoinMessageEntity, BitcoinMessageEntityMapper>();

            var entities = await repository.Get()
                .Include(p => p.Sender)
                .Include(p => p.Recipient)
                .Include(p => p.BitcoinMessageInventories)
                .Include(p => p.BitcoinMessageTransactions)
                .ThenInclude(ppt => ppt.BitcoinTransaction)
                .ThenInclude(t => t.Inputs)
                .Include(p => p.BitcoinMessageTransactions)
                .ThenInclude(ppt => ppt.BitcoinTransaction)
                .ThenInclude(t => t.Outputs)
                .Include(p => p.BitcoinMessageHeaders)
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

    public async Task<IEnumerable<BitcoinMessageListModel>> GetPacketListByInventoryIdAndAnalysisIdAsync(
        Guid inventoryId, Guid fileAnalysisId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.BitcoinMessages;

        var entities = await repository.GetByInventoryIdAndAnalysisIdAsync(inventoryId, fileAnalysisId);

        return entities.Select(entity => ModelMapper.MapToListModel(entity)).ToList();
    }
}
