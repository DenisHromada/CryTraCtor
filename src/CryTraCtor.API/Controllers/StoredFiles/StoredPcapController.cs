using CryTraCtor.Facades;
using CryTraCtor.Models.StoredFiles;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.StoredFiles;

[ApiController]
[Route("stored-files")]
public class StoredPcapController(
    IStoredFileFacade storedFileFacade
) : Controller
{
    [HttpGet("Index")]
    public IEnumerable<StoredFileListModel> GetStoredPcapFiles()
    {
        return storedFileFacade.GetAll();
    }

    [HttpPost]
    public async Task<IActionResult> PostStoredPcapFile(IFormFile file)
    {
        try
        {
            var storedFileName = await storedFileFacade.Store(file);
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
        await storedFileFacade.Delete(fileName);
        return Ok("Successfully deleted file: " + fileName);
    }
}