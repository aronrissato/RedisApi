using System.Runtime.InteropServices.ComTypes;
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
        // db.StringSet(platform.Id, serialPlatform);
        // db.SetAdd("PlatformSet", serialPlatform);
        
        db.HashSet("hashplatform", new HashEntry[]
        {new HashEntry(platform.Id, serialPlatform)});
    }

    public IEnumerable<Platform?>? GetAll()
    {
        var db = _redis.GetDatabase();
        // var completeSet = db.SetMembers("PlatformSet");
        
        var completeHash = db.HashGetAll("hashplatform");
        
        if (completeHash.Length <= 0) return null;
        
        var obj = Array.ConvertAll(
            completeHash, value
                => JsonSerializer.Deserialize<Platform>(value.Value)).ToList();
        return obj;
    }

    public Platform? Get(string id)
    {
        var db = _redis.GetDatabase();

        // var platform = db.StringGet(id);

        var platform = db.HashGet("hashplatform", id);
        
        return !string.IsNullOrEmpty(platform)
            ? JsonSerializer.Deserialize<Platform>(platform)
            : null;
    }
}