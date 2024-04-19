using CryTraCtor.Database.Entities;
using CryTraCtor.Mappers.ModelMapperBase;
using CryTraCtor.Models.KnownDomain;

namespace CryTraCtor.Mappers.KnownDomain;

public class KnownDomainListModelMapper : ListModelMapperBase<KnownDomainEntity, KnownDomainListModel>
{  
    public override KnownDomainListModel MapToListModel(KnownDomainEntity? entity)
        => entity is null
            ? KnownDomainListModel.Empty()
            : new KnownDomainListModel { Id = entity.Id, DomainName = entity.DomainName };
}