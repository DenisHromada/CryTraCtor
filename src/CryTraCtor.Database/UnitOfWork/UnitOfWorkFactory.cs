using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<CryTraCtorDbContext> dbContextFactory) : IUnitOfWorkFactory
{
   public IUnitOfWork Create() => new UnitOfWork(dbContextFactory.CreateDbContext());

}