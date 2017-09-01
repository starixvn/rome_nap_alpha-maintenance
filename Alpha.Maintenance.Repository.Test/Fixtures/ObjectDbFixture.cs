namespace Alpha.Maintenance.Repository.Test.Fixtures
{
    using Fanex.Data;
    using NSubstitute;

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
