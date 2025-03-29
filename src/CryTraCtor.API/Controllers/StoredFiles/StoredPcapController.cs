using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.StoredFiles;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.StoredFiles;

[ApiController]
[Route("stored-files")]
public class StoredPcapController(
    IStoredFileFacade storedFileFacade
) : Controller
{
    [HttpGet("Index")]
    public async Task<IEnumerable<StoredFileListModel>> GetStoredPcapFiles()
    {
        return await storedFileFacade.GetAllAsync();
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
    public async Task<IActionResult> PostStoredPcapFile(IFormFile file)
    {
        try
        {
            var stream = file.OpenReadStream();
            var storedFileCreateModel = new StoredFileCreateModel(file.FileName, file.ContentType, file.Length);
            var storedFileName = await storedFileFacade.StoreAsync(storedFileCreateModel, stream);
            return Ok("Successfully stored file: " + storedFileName);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> RenameStoredPcapFile(string oldFileName, string newFileName)
    {
        try
        {
            var storedFileName = await storedFileFacade.RenameAsync(oldFileName, newFileName);
            return Ok("Successfully stored file: " + storedFileName);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteStoredPcapFile(string fileName)
    {
        await storedFileFacade.DeleteAsync(fileName);
        return Ok("Successfully deleted file: " + fileName);
    }
}
