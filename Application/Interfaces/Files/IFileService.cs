using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Files;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file);
    Task DeleteAsync(string path);
    Stream? GetFile(string fileName);
}
