namespace Alpha.Maintenance.Service.Impl.AdminTools
{
    using Alpha.Maintenance.Service.Interface.AdminTools;
    using Couchbase;

    public class CouchbaseFactoryImpl : CouchbaseFactory
    {
        public ICouchbaseClient CreateClient(string bucket)
        {
            return new CouchbaseClient(bucket, string.Empty);
        }
    }
}
