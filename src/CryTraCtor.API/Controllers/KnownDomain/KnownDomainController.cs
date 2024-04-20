using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.KnownDomain;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.KnownDomain;

[ApiController]
[Route("known-domain")]
public class KnownDomainController(
    IKnownDomainFacade knownDomainFacade
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
}