using Application.Interfaces.Files;
using Application.Responses;
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

        return Ok(new ApiResponse<String>
        {
            Data = fileName,
            Errors = null,
            Message = "Success",
            StatusCode = 200,
            Success = true,
            TotalRecords = 1
        });
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(String filename)
    {
        await _fileService.DeleteAsync(filename);
        return Ok(new ApiResponse<Task>
        {
            Data = Task.CompletedTask,
            Errors = null,
            Message = "Success",
            StatusCode = 200,
            Success = true,
            TotalRecords = 1
        });
    }
    [HttpGet]
    public async Task<IActionResult> GetFile(String filename)
    {
        var stream = _fileService.GetFile(filename);

        if (stream == null)
            return NotFound();
        return Ok(new ApiResponse<FileStreamResult>
        {
            TotalRecords = 1,
            Success = true,
            StatusCode = 200,
            Message = "Success",
            Data = File(stream, "application/octet-stream", filename),
            Errors = null
        });
    }
}
