namespace CryTraCtor.Database.UnitOfWork;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}