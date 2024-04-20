namespace CryTraCtor.Business.Mappers;

public interface IListModelMapper<TEntity, out TListModel>
{
    TListModel MapToListModel(TEntity? entity);

    IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities);
}