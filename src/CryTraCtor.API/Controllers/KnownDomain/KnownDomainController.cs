using System.Collections.ObjectModel;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.KnownDomain;

[ApiController]
[Route("known-domain")]
public class KnownDomainController(
    IKnownDomainFacade knownDomainFacade,
    IKnownDomainImportFacade knownDomainImportFacade,
    CsvService csvService
) : Controller
{
    [HttpGet("Index")]
    public async Task<IEnumerable<KnownDomainListModel>> Get()
    {
        return await knownDomainFacade.GetAllAsync();
    }

    [HttpPut("CreateOrUpdate")]
    public async Task<IActionResult> Put(KnownDomainDetailModel knownDomainDetailModel)
    {
        try
        {
            await knownDomainFacade.CreateOrUpdateAsync(knownDomainDetailModel);
            return Ok("Successfully created or updated known domain: " + knownDomainDetailModel.DomainName);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await knownDomainFacade.DeleteAsync(id);
        return Ok("Successfully deleted known domain with id: " + id);
    }
    
    [HttpPost("Import")]
    public async Task<IActionResult> Import(Collection<KnownDomainImportModel> modelCollection)
    {
        try
        {
            await knownDomainImportFacade.Create(modelCollection);
            return Ok("Successfully imported known domains");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("Import/csv")]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        var stream = file.OpenReadStream();
        var modelCollection = csvService.ParseCsv(stream);
        try
        {
            await knownDomainImportFacade.Create(modelCollection);
            return Ok("Successfully imported known domains");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
}