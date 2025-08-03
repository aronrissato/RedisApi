using Microsoft.AspNetCore.Mvc;
using RedisApi.Data;
using RedisApi.Model;

namespace RedisApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;

    public PlatformsController(IPlatformRepo repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public IActionResult Create(Platform platform)
    {
        _repo.Create(platform);
        return Ok();
    }

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

    [HttpGet]
    public ActionResult<Platform> GetById() =>
        Ok(_repo.GetAll());
}