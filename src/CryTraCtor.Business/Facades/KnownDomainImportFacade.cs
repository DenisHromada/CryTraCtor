using System.Collections.ObjectModel;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.KnownDomain;
using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class KnownDomainImportFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    KnownDomainImportModelMapper mapper
) : IKnownDomainImportFacade
{
    public async Task Create(Collection<KnownDomainImportModel> modelCollection)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        foreach (var model in modelCollection)
        {
            var (cryptoProduct, domainName) = mapper.MapToEntity(model);
            var cryptoProductRepository = unitOfWork.GetRepository<CryptoProductEntity, CryptoProductEntityMapper>();
            var existingProduct = await cryptoProductRepository.Get().FirstOrDefaultAsync(product =>
                product.ProductName == cryptoProduct.ProductName
                && product.Vendor == cryptoProduct.Vendor);
            if (existingProduct is null)
            {
                existingProduct = await cryptoProductRepository.InsertAsync(cryptoProduct);
            }
            domainName.CryptoProductId = existingProduct.Id;
            
            var knownDomainRepository = unitOfWork.GetRepository<KnownDomainEntity, KnownDomainEntityMapper>();
            var existingDomainName = await knownDomainRepository.Get().FirstOrDefaultAsync(
                domain => domain.DomainName == domainName.DomainName
                && domain.Purpose == domainName.Purpose
                & domain.Description == domainName.Description
                && domain.CryptoProductId == domainName.CryptoProductId
                );
            if (existingDomainName is null)
            {
                await knownDomainRepository.InsertAsync(domainName);
            }

        }
        await unitOfWork.CommitAsync();
    }
}