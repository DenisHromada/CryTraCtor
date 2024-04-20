using CryTraCtor.Business.Models.CryptoProduct;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface ICryptoProductFacade : IFacade<CryptoProductEntity, CryptoProductListModel, CryptoProductDetailModel>
{
    
}