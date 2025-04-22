using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public abstract class
    FacadeBase<TEntity, TListModel, TDetailModel, TEntityMapper>(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TDetailModel> modelMapper
    ) : IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
{
    protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper = modelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;

    protected virtual ICollection<string> IncludesNavigationPathDetail => new List<string>();

    public virtual async Task DeleteAsync(Guid id)
    {
        var unitOfWork = UnitOfWorkFactory.Create();
        try
        {
            await unitOfWork.GetRepository<TEntity, TEntityMapper>().DeleteAsync(id).ConfigureAwait(false);
            await unitOfWork.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Could not delete entity", e);
        }
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using var unitOfWork = UnitOfWorkFactory.Create();
        var query = unitOfWork
            .GetRepository<TEntity, TEntityMapper>()
            .Get();

        foreach (var includePath in IncludesNavigationPathDetail)
        {
            query = query.Include(includePath);
        }

        var entity = await query.SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

    public virtual async Task<IEnumerable<TListModel>> GetAllAsync()
    {
        var unitOfWork = UnitOfWorkFactory.Create();
        var entities = await unitOfWork
            .GetRepository<TEntity, TEntityMapper>()
            .Get()
            .ToListAsync()
            .ConfigureAwait(false);
        return ModelMapper.MapToListModel(entities);
    }

    public virtual async Task<TDetailModel> CreateOrUpdateAsync(TDetailModel model)
    {
        TDetailModel result;
        var entity = ModelMapper.MapToEntity(model);

        var unitOfWork = UnitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity).ConfigureAwait(false))
        {
            var updatedEntity = await repository.UpdateAsync(entity).ConfigureAwait(false);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            var insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await unitOfWork.CommitAsync().ConfigureAwait(false);
        return result;
    }
}
