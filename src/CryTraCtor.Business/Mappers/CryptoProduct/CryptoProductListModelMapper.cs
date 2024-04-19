using CryTraCtor.Database.Entities;
using CryTraCtor.Mappers.ModelMapperBase;
using CryTraCtor.Models.CryptoProduct;

namespace CryTraCtor.Mappers.CryptoProduct;

public class CryptoProductListModelMapper : ListModelMapperBase<CryptoProductEntity, CryptoProductListModel>
{
    public override CryptoProductListModel MapToListModel(CryptoProductEntity? entity)
        => entity is null
            ? CryptoProductListModel.Empty()
            : new CryptoProductListModel { Id = entity.Id, Vendor = entity.Vendor, ProductName = entity.ProductName };
    
}