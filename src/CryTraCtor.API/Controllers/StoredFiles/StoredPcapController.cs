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
    IFileStorageConfig configuration)
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
        var size = file.Length;
        if (size <= 0)
        {
            return BadRequest("File is empty");
        }

        var fileDbEntity = new StoredFile
        {
            Id = new Guid(),
            PublicFileName = file.FileName,
            MimeType = file.ContentType,
            FileSize = size,
            InternalFilePath = GetInternalFilePath()
        };

        await using var stream = System.IO.File.Create(fileDbEntity.InternalFilePath);
        await file.CopyToAsync(stream);

        return Ok("File uploaded successfully");

        string GetInternalFilePath()
        {
            var localFileStorageDirectory = configuration.CaptureFileDirectory;
            var internalFileName = Path.GetRandomFileName();
            return Path.Combine(localFileStorageDirectory, internalFileName);
        }
    }
}