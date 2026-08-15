using Application.Interfaces.Cache;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CacheController : ControllerBase
{
    private readonly ICacheService _cache;

    public CacheController(ICacheService cache)
    {
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> Set(String key, int minutes, String jsonContent)
    {
        await _cache.SetAsync(
            key,
            jsonContent,
            minutes);

        return Ok(new ApiResponse<Task>
        {
            Data = Task.CompletedTask,
            Errors = null,
            Message = "Saved",
            StatusCode = 200,
            Success = true,
            TotalRecords = 1
        });
    }

    [HttpGet]
    public async Task<IActionResult> Getcache(String Key)
    {
        var value = await _cache.GetAsync<string>(Key);

        if (value == null)
            return NotFound();

        return Ok(new ApiResponse<String>
        {
            Data = value,
            TotalRecords = 1,
            Success = true,
            StatusCode = 200,
            Errors =null,
            Message = "Retrived Successfully"
        });
    }
    [HttpDelete("{key}")]
    public async Task<IActionResult> Remove(string key)
    {
        await _cache.RemoveAsync(key);

        return Ok(new ApiResponse<Task>
        {
            Data = Task.CompletedTask,
            Message = "Cache removed successfully.",
            Errors = null,
            StatusCode = 200,
            Success = true,
            TotalRecords = 0
        });
    }
}