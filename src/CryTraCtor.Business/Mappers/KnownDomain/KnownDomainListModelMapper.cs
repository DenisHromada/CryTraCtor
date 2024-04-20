using CryTraCtor.Business.Mappers.ModelMapperBase;
using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.KnownDomain;

public class KnownDomainListModelMapper : ListModelMapperBase<KnownDomainEntity, KnownDomainListModel>
{  
    public override KnownDomainListModel MapToListModel(KnownDomainEntity? entity)
        => entity is null
            ? KnownDomainListModel.Empty()
            : new KnownDomainListModel { Id = entity.Id, DomainName = entity.DomainName };
}