using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
namespace Infrastructure.Files;

public class FileService:IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadAsync(
        IFormFile file)
    {
        if(_env.WebRootPath is null)
        {
            _env.WebRootPath = _env.ContentRootPath;
        }
        var folder =
            Path.Combine(
                _env.WebRootPath,
                "Documents");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fileName =
            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var path =
            Path.Combine(folder, fileName);

        using var stream =
            new FileStream(path, FileMode.Create);

        await file.CopyToAsync(stream);

        return fileName;
    }

    public Stream? GetFile(string fileName)
    {
        if (_env.WebRootPath is null)
        {
            _env.WebRootPath = _env.ContentRootPath;
        }

        var path = Path.Combine(
            _env.WebRootPath,
            "Documents",
            fileName);

        if (!File.Exists(path))
            return null;

        return new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read);
    }

    public Task DeleteAsync(string fileName)
    {
        if (_env.WebRootPath is null)
        {
            _env.WebRootPath = _env.ContentRootPath;
        }
        var path =
            Path.Combine(
                _env.WebRootPath,
                "Documents",
                fileName);

        if (File.Exists(path))
            File.Delete(path);

        return Task.CompletedTask;
    }
}
