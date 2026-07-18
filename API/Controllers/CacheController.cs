using InfraStructure.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        return Ok("Saved");
    }

    [HttpGet]
    public async Task<IActionResult> Getcache(String Key)
    {
        var value = await _cache.GetAsync<string>(Key);

        if (value == null)
            return NotFound();

        return Ok(value);
    }
    [HttpDelete("{key}")]
    public async Task<IActionResult> Remove(string key)
    {
        await _cache.RemoveAsync(key);

        return Ok("Cache removed successfully.");
    }
}