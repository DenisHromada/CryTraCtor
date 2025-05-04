using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.UnitOfWork;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace CryTraCtor.Business.Services;

public class BitcoinAnalysisService(
    IUnitOfWorkFactory unitOfWorkFactory,
    ILogger<BitcoinAnalysisService> logger,
    BitcoinPacketAnalyzer bitcoinPacketAnalyzer,
    ITrafficParticipantFacade trafficParticipantFacade,
    IMemoryCache memoryCache)
{
    private const int BatchSize = 500;

    public async Task AnalyzeAsync(StoredFileDetailModel storedFile, Guid fileAnalysisId)
    {
        logger.LogInformation(
            "[BitcoinAnalysisService] Starting Bitcoin analysis for FileAnalysisId: {FileAnalysisId} on file: {FilePath}",
            fileAnalysisId, storedFile.InternalFilePath);

        var stopwatch = Stopwatch.StartNew();
        IEnumerable<IBitcoinPacketSummary> bitcoinMessages;
        try
        {
            bitcoinMessages = bitcoinPacketAnalyzer.AnalyzeFromFile(storedFile.InternalFilePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[BitcoinAnalysisService] Error processing Bitcoin data chunks from {InternalFilePath} for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                storedFile.InternalFilePath, fileAnalysisId, ex.Message);
            return;
        }

        await using var uow = unitOfWorkFactory.Create();
        var packetRepository =
            uow.GetRepository<BitcoinMessageEntity,
                Database.Mappers.BitcoinPacketEntityMapper>();

        int processedMessageCount = 0;
        int currentBatchCount = 0;

        async Task<Guid?> GetParticipantIdAsync((string Address, int Port) endpointKey)
        {
            var cacheKey = $"ParticipantLookup_{fileAnalysisId}_{endpointKey.Address}_{endpointKey.Port}";

            if (memoryCache.TryGetValue(cacheKey, out Guid participantId))
            {
                return participantId;
            }

            var participant = await trafficParticipantFacade.GetByAddressPortAndFileAnalysisIdAsync(
                fileAnalysisId,
                endpointKey.Address,
                endpointKey.Port
            );

            if (participant != null)
            {
                memoryCache.Set(cacheKey, participant.Id);
                return participant.Id;
            }

            return null;
        }

        foreach (var messageSummary in bitcoinMessages)
        {
            try
            {
                if (messageSummary is not BitcoinMessageSummary concreteSummary)
                {
                    logger.LogWarning(
                        "[BitcoinAnalysisService] Received unexpected summary type for FileAnalysisId: {FileAnalysisId}. Skipping.",
                        fileAnalysisId);
                    continue;
                }

                var senderId = await GetParticipantIdAsync((concreteSummary.Source.Address,
                    concreteSummary.Source.Port));
                var recipientId = await GetParticipantIdAsync((concreteSummary.Destination.Address,
                    concreteSummary.Destination.Port));

                if (senderId == null || recipientId == null)
                {
                    logger.LogWarning(
                        "[BitcoinAnalysisService] Could not find sender ({SourceAddress}:{SourcePort}) or recipient ({DestinationAddress}:{DestinationPort}) participant ID in lookup for FileAnalysisId: {FileAnalysisId}. Skipping Bitcoin message: {Command}",
                        concreteSummary.Source.Address, concreteSummary.Source.Port,
                        concreteSummary.Destination.Address,
                        concreteSummary.Destination.Port, fileAnalysisId, concreteSummary.Command);
                    continue;
                }

                var packetEntity = new BitcoinMessageEntity
                {
                    Id = Guid.NewGuid(),
                    FileAnalysisId = fileAnalysisId,
                    SenderId = senderId.Value,
                    RecipientId = recipientId.Value,
                    Timestamp = concreteSummary.Timestamp,
                    Magic = concreteSummary.Magic,
                    Command = concreteSummary.Command,
                    Length = concreteSummary.PayloadSize,
                    Checksum = concreteSummary.Checksum
                };

                switch (concreteSummary.Command)
                {
                    case "inv" or "getdata" or "notfound"
                        when concreteSummary.Inventories != null && concreteSummary.Inventories.Count != 0:
                        await HandleInventoryRelatedMessageAsync(packetEntity, concreteSummary, uow);
                        break;
                    case "tx" when concreteSummary.Transaction != null:
                        await HandleTransactionMessageAsync(packetEntity, concreteSummary, uow);
                        break;
                    case "headers" when concreteSummary.Headers != null && concreteSummary.Headers.Count != 0:
                        await HandleHeadersMessageAsync(packetEntity, concreteSummary, uow);
                        break;
                }

                await packetRepository.InsertAsync(packetEntity);

                processedMessageCount++;
                currentBatchCount++;

                if (currentBatchCount >= BatchSize)
                {
                    await uow.CommitAsync();
                    currentBatchCount = 0;
                    logger.LogDebug("[BitcoinAnalysisService] Committed batch for FileAnalysisId: {FileAnalysisId}",
                        fileAnalysisId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(0, ex,
                    "[BitcoinAnalysisService] Error processing Bitcoin message summary (Command: {Command}) for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                    messageSummary?.GetSerializedPacketString() ?? "unknown", fileAnalysisId,
                    ex.Message);
            }
        }

        if (currentBatchCount > 0)
        {
            await uow.CommitAsync();
            logger.LogDebug("[BitcoinAnalysisService] Committed final batch for FileAnalysisId: {FileAnalysisId}",
                fileAnalysisId);
        }

        stopwatch.Stop();
        logger.LogInformation(
            "[BitcoinAnalysisService] Completed Bitcoin analysis for FileAnalysisId: {FileAnalysisId}. Processed {ProcessedMessageCount} messages in {ElapsedMilliseconds} ms.",
            fileAnalysisId, processedMessageCount, stopwatch.ElapsedMilliseconds);
    }

    private async Task HandleInventoryRelatedMessageAsync(BitcoinMessageEntity messageEntity,
        BitcoinMessageSummary summary, IUnitOfWork uow)
    {
        foreach (var invItem in summary.Inventories)
        {
            var inventoryEntity =
                await uow.BitcoinInventories.GetOrCreateAsync(invItem.Type.ToString(),
                    invItem.Hash.ToString());
            messageEntity.BitcoinPacketInventories.Add(new BitcoinPacketInventoryEntity
            {
                BitcoinMessage = messageEntity,
                BitcoinInventory = inventoryEntity
            });
        }
    }

    private async Task HandleTransactionMessageAsync(BitcoinMessageEntity messageEntity, BitcoinMessageSummary summary,
        IUnitOfWork uow)
    {
        var transactionRepository = uow.BitcoinTransactions;
        var transactionEntity =
            await transactionRepository.GetByTxIdAsync(summary.Transaction.GetHash()
                .ToString());
        if (transactionEntity == null)
        {
            transactionEntity = new BitcoinTransactionEntity
            {
                Id = Guid.NewGuid(),
                TxId = summary.Transaction.GetHash().ToString(),
                Version = (int)summary.Transaction.Version,
                Locktime = summary.Transaction.LockTime.Value,
                Inputs = summary.Transaction.Inputs
                    .Select(txIn => new BitcoinTransactionInputEntity
                    {
                        Id = Guid.NewGuid(),
                        PreviousTxHash = txIn.PrevOut.Hash.ToString(),
                        PreviousOutputIndex = txIn.PrevOut.N,
                        ScriptSig = txIn.ScriptSig.ToHex(),
                        Sequence = txIn.Sequence.Value
                    })
                    .ToList(),
                Outputs = summary.Transaction.Outputs
                    .Select(txOut => new BitcoinTransactionOutputEntity
                    {
                        Id = Guid.NewGuid(),
                        Value = txOut.Value.Satoshi,
                        ScriptPubKey = txOut.ScriptPubKey.ToHex()
                    })
                    .ToList()
            };
            await transactionRepository.InsertAsync(transactionEntity);
        }

        messageEntity.BitcoinPacketTransactions.Add(new BitcoinPacketTransactionEntity
        {
            BitcoinMessage = messageEntity,
            BitcoinTransaction = transactionEntity
        });
    }

    private async Task HandleHeadersMessageAsync(BitcoinMessageEntity messageEntity, BitcoinMessageSummary summary,
        IUnitOfWork uow)
    {
        var blockHeaderRepository = uow.BitcoinBlockHeaders;
        foreach (var header in summary.Headers)
        {
            var blockHeaderEntity = await blockHeaderRepository.GetOrCreateByBlockHashAsync(
                header.GetHash().ToString(), new BitcoinBlockHeaderEntity
                {
                    BlockHash = header.GetHash().ToString(),
                    Version = header.Version,
                    PrevBlockHash = header.HashPrevBlock.ToString(),
                    MerkleRoot = header.HashMerkleRoot.ToString(),
                    Timestamp = header.BlockTime,
                    Bits = header.Bits,
                    Nonce = header.Nonce
                });
            messageEntity.BitcoinPacketHeaders.Add(new BitcoinPacketHeaderEntity
            {
                BitcoinMessage = messageEntity,
                BitcoinBlockHeader = blockHeaderEntity
            });
        }
    }
}
