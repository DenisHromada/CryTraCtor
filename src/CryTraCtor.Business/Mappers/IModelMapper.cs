namespace CryTraCtor.Mappers;

public interface IModelMapper<TEntity, out TListModel, TDetailModel> :
    IListModelMapper<TEntity, TListModel>
{
    TDetailModel MapToDetailModel(TEntity entity);
    TEntity MapToEntity(TDetailModel model);
}