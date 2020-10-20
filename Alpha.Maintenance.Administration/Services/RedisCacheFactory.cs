namespace Alpha.Maintenance.Administration.Services
{
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using StackExchange.Redis;
    using System.Collections.Generic;
    using System.Configuration;

    public interface IRedisCacheFactory
    {
        ICacheService Create(string settingKey);
    }

    public class RedisCacheFactory : IRedisCacheFactory
    {
        public static IRedisCacheFactory DefaultInstance { get; } = new RedisCacheFactory();

        private readonly IDictionary<string, ICacheService> cache = new Dictionary<string, ICacheService>();

        public ICacheService Create(string settingKey)
        {
            if (!cache.ContainsKey(settingKey))
            {
                lock (cache)
                {
                    if (!cache.ContainsKey(settingKey))
                    {
                        var cacheService = InternalCreate(settingKey);
                        cache.Add(settingKey, cacheService);
                        return cacheService;
                    }
                }
            }

            return cache[settingKey];
        }

        private ICacheService InternalCreate(string settingKey)
        {
            var redisConfigurationString = ConfigurationManager.AppSettings[settingKey];
            var configurationOptions = ConfigurationOptions.Parse(redisConfigurationString);
            var redisCacheOptions = new RedisCacheOptions
            {
                ApplicationName = configurationOptions.ClientName,
                ConnectionOptions = configurationOptions
            };
            return new CacheService(new RedisCache(redisCacheOptions));
        }
    }
}
