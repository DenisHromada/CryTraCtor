namespace CryTraCtor.Business.Mappers.ModelMapperBase;

public abstract class ModelMapperBase<TEntity, TListModel, TDetailModel>() :
    ListModelMapperBase<TEntity, TListModel>,
    IModelMapper<TEntity, TListModel, TDetailModel>
{
    public abstract TDetailModel MapToDetailModel(TEntity entity);
    public abstract TEntity MapToEntity(TDetailModel model);
}