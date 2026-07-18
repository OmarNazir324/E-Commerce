using Infrastructure.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UploadDocumentController : ControllerBase
{
    private readonly IFileService _fileService;
    public UploadDocumentController(IFileService fileService)
    {
        this._fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(
           IFormFile file)
    {
        var fileName =
            await _fileService.UploadAsync(file);

        return Ok(fileName);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(String filename)
    {
        await _fileService.DeleteAsync(filename);
        return Ok(Task.CompletedTask);
    }
    [HttpGet]
    public async Task<IActionResult> GetFile(String filename)
    {
        var stream = _fileService.GetFile(filename);

        if (stream == null)
            return NotFound();

        return File(
            stream,
            "application/octet-stream",
            filename);
    }
}
