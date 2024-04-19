using CryTraCtor.Facades.Interfaces;
using CryTraCtor.Models.CryptoProduct;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.CryptoProductController;

[ApiController]
[Route("crypto-product")]
public class CryptoProductController(
    ICryptoProductFacade cryptoProductFacade
) : Controller
{
    [HttpGet("Index")]
    public async Task<IEnumerable<CryptoProductListModel>> Get()
    {
        return await cryptoProductFacade.GetAllAsync();
    }

    [HttpPut("CreateOrUpdate")]
    public async Task<IActionResult> Put(CryptoProductDetailModel cryptoProductDetailModel)
    {
        try
        {
            await cryptoProductFacade.CreateOrUpdateAsync(cryptoProductDetailModel);
            return Ok("Successfully created or updated crypto product: " + cryptoProductDetailModel.ProductName);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await cryptoProductFacade.DeleteAsync(id);
        return Ok("Successfully deleted crypto product with id: " + id);
    }
}