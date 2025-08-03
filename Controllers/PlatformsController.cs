using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedisApi.Data;
using RedisApi.Model;

namespace RedisApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;

    public PlatformsController(IPlatformRepo repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public ActionResult<Platform> Create(Platform platform)
    {
        _repo.Create(platform);
        return CreatedAtRoute(
            "GetPlatformById",
            new { id = platform.Id },
            platform);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Platform>> GetAll() =>
        Ok(_repo.GetAll());


    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<Platform> Get(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest();
        var platform = _repo.Get(id);
        return platform is null
            ? NotFound()
            : Ok(platform);
    }
}