namespace CryTraCtor.Mappers.ModelMapperBase;

public abstract class ListModelMapperBase<TEntity, TListModel> : IListModelMapper<TEntity, TListModel>
{
    public abstract TListModel MapToListModel(TEntity? entity);

    public virtual IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToListModel);

}