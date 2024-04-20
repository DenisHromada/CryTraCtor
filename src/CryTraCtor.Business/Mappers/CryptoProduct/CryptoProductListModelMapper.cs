using CryTraCtor.Business.Mappers.ModelMapperBase;
using CryTraCtor.Business.Models.CryptoProduct;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.CryptoProduct;

public class CryptoProductListModelMapper : ListModelMapperBase<CryptoProductEntity, CryptoProductListModel>
{
    public override CryptoProductListModel MapToListModel(CryptoProductEntity? entity)
        => entity is null
            ? CryptoProductListModel.Empty()
            : new CryptoProductListModel { Id = entity.Id, Vendor = entity.Vendor, ProductName = entity.ProductName };
    
}