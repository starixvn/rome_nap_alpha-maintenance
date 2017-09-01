namespace Alpha.Maintenance.Service.Test.Fixtures
{
    using Fanex.Data;
    using NSubstitute;
    using Repository;

    public class ObjectDbFixture
    {
        public ObjectDbFixture()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.CreateInstance = (settingKey) => ObjectDb;
        }

        public IObjectDb ObjectDb { get; private set; }
    }
}
