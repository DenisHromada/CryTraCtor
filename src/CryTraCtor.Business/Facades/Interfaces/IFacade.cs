using CryTraCtor.Database.Entities;
using CryTraCtor.Models;

namespace CryTraCtor.Facades.Interfaces;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
{
    public Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<IEnumerable<TListModel>> GetAllAsync();
    Task<TDetailModel> CreateOrUpdateAsync(TDetailModel model);
}