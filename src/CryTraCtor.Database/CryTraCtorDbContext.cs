using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database;

public class CryTraCtorDbContext : DbContext
{
    public virtual DbSet<StoredFileEntity> StoredFile { get; set; }
    public virtual DbSet<CryptoProductEntity> CryptoProduct { get; set; }
    public virtual DbSet<KnownDomainEntity> KnownDomain { get; set; }
    public virtual DbSet<TrafficParticipantEntity> TrafficParticipant { get; set; }
    public virtual DbSet<FileAnalysisEntity> FileAnalysis { get; set; }
    public virtual DbSet<DnsMessageEntity> DnsMessage { get; set; }
    public virtual DbSet<DomainMatchEntity> DomainMatch { get; set; }
    public virtual DbSet<BitcoinMessageEntity> BitcoinMessage { get; set; }
    public virtual DbSet<BitcoinInventoryEntity> BitcoinInventory { get; set; }
    public virtual DbSet<BitcoinMessageInventoryEntity> BitcoinMessageInventory { get; set; }
    public virtual DbSet<BitcoinTransactionEntity> BitcoinTransaction { get; set; }
    public virtual DbSet<BitcoinTransactionInputEntity> BitcoinTransactionInput { get; set; }
    public virtual DbSet<BitcoinTransactionOutputEntity> BitcoinTransactionOutput { get; set; }
    public virtual DbSet<BitcoinMessageTransactionEntity> BitcoinMessageTransaction { get; set; }
    public virtual DbSet<BitcoinBlockHeaderEntity> BitcoinBlockHeader { get; set; }
    public virtual DbSet<BitcoinMessageHeaderEntity> BitcoinMessageHeader { get; set; }

    public CryTraCtorDbContext()
    {
    }

    public CryTraCtorDbContext(DbContextOptions<CryTraCtorDbContext> options)
        : base(options)
    {
    }
}
