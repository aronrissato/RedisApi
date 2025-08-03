using System.Text.Json;
using RedisApi.Model;
using StackExchange.Redis;

namespace RedisApi.Data;

public class RedisPlatformRepo : IPlatformRepo
{
    private readonly IConnectionMultiplexer _redis;

    public RedisPlatformRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public void Create(Platform platform)
    {
        if (platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        var db = _redis.GetDatabase();
        var serialPlatform = JsonSerializer.Serialize(platform);
        db.StringSet(platform.Id, serialPlatform);
        
        db.SetAdd("PlatformSet", serialPlatform);
    }

    public IEnumerable<Platform> GetAll()
    {
        throw new NotImplementedException();
    }

    public Platform? Get(string id)
    {
        var db = _redis.GetDatabase();
        var platform = db.StringGet(id);

        return !string.IsNullOrEmpty(platform)
            ? JsonSerializer.Deserialize<Platform>(platform)
            : null!;
    }
}