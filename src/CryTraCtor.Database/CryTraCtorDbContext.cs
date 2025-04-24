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
    public virtual DbSet<DnsPacketEntity> DnsPacket { get; set; }
    public virtual DbSet<DomainMatchEntity> DomainMatch { get; set; }
    public CryTraCtorDbContext()
    {
    }

    public CryTraCtorDbContext(DbContextOptions<CryTraCtorDbContext> options)
        : base(options)
    {
    }
}
