namespace Alpha.Maintenance.Repository.Test.Fixtures
{
    using Fanex.Caching;
    using NSubstitute;

    public class FanexCacheFixture
    {
        public FanexCacheFixture()
        {
            CacheForTesting = Substitute.For<Cache>();
            ConfiguredCacheService.SwitchCacheStorage(CacheForTesting);
        }

        public Cache CacheForTesting { get; private set; }
    }
}
