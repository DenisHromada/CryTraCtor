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
    public IEnumerable<StoredFileListModel> GetStoredPcapFiles()
    {
        return storedFileFacade.GetAll();
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
    public async Task<IActionResult> PostStoredPcapFile(IFormFile file)
    {
        try
        {
            var stream = file.OpenReadStream();
            var storedFileDetailModel = StoredFileDetailModel.Empty() with
            {
                PublicFileName = file.FileName,
                FileSize = file.Length,
                MimeType = file.ContentType
            };
            var storedFileName = await storedFileFacade.StoreAsync(storedFileDetailModel, stream);
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
            var storedFileName = await storedFileFacade.Rename(oldFileName, newFileName);
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
