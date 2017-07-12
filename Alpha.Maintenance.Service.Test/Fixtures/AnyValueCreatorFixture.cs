namespace Alpha.Maintenance.Service.Test.Fixtures
{
    using Ploeh.AutoFixture;

    public class AnyValueCreatorFixture
    {
        private readonly Fixture anyValueGenerator;

        public AnyValueCreatorFixture()
        {
            anyValueGenerator = new Fixture();
        }

        public T Create<T>() => anyValueGenerator.Create<T>();
    }
}
