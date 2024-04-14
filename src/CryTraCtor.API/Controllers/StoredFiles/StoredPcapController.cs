using CryTraCtor.Database;
using CryTraCtor.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.APi.Controllers.StoredFiles;

[ApiController]
[Route("stored-files")]
public class StoredPcapController(IDbContextFactory<CryTraCtorDbContext> contextFactory) : Controller
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
   public async Task<ActionResult<string>> PostStoredPcapFile([FromBody] StoredFile storedFile)
   {
      await using var dbContext = await contextFactory.CreateDbContextAsync();
      await dbContext.StoredFiles.AddAsync(storedFile);
      await dbContext.SaveChangesAsync();
      return Ok();
   }
}