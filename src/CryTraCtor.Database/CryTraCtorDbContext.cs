using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database;

public class CryTraCtorDbContext : DbContext
{
    public virtual DbSet<StoredFileEntity> StoredFiles { get; set; }

    public CryTraCtorDbContext()
    {
    }

    public CryTraCtorDbContext(DbContextOptions<CryTraCtorDbContext> options)
        : base(options)
    {
    }
}