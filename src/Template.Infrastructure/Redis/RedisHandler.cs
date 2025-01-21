using System;
using Cache.Redis;
using Cache.Redis.Handler;
using Cache.Redis.Handler.Interface;
using Cache.Redis.Factory;
using StackExchange.Redis;

namespace Template.Infrastructure.Redis
{
    public class RedisHandler : IRedisHandler
    {
        private readonly ICacheRedisHandler _redisCache;

        public RedisHandler(RedisConfig config)
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(config.ConnectionString);
            _redisCache = new CacheRedisHandler(connectionMultiplexer);
        }

        public void SetValue(string key, string value, TimeSpan expiry)
        {
            _redisCache.SetValue(key, value, expiry);
        }

        public string GetValue(string key)
        {
            return _redisCache.GetValue(key);
        }

        public bool KeyExists(string key)
        {
            return _redisCache.KeyExists(key);
        }

        public bool RemoveKey(string key)
        {
            return _redisCache.RemoveKey(key);
        }
    }

    public interface IRedisHandler
    {
        void SetValue(string key, string value, TimeSpan expiry);
        string GetValue(string key);
        bool KeyExists(string key);
        bool RemoveKey(string key);
    }

    public class RedisConfig
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
    }
}
