using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database;

public class CryTraCtorDbContext : DbContext
{
    public virtual DbSet<StoredFile> StoredFiles { get; set; }
    public CryTraCtorDbContext(DbContextOptions<CryTraCtorDbContext> options, bool seedData = false) : base(options)
    {
    }
    

}