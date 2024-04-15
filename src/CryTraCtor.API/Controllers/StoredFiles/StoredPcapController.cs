using CryTraCtor.APi.Services;
using CryTraCtor.Database;
using CryTraCtor.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.APi.Controllers.StoredFiles;

[ApiController]
[Route("stored-files")]
public class StoredPcapController(
    IDbContextFactory<CryTraCtorDbContext> contextFactory,
    IFileStorageService fileStorageService)
    : Controller
{
    [HttpGet("Index")]
    public async Task<ActionResult<string>> GetStoredPcapFiles()
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var result = await dbContext.StoredFiles
            .ToListAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostStoredPcapFile(IFormFile file)
    {
        try
        {
            await fileStorageService.StoreFileAsync(file);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteStoredPcapFile(string fileName)
    {
        try
        {
            await fileStorageService.DeleteFileAsync(fileName);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}