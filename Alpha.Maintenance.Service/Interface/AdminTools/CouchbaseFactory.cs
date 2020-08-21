namespace Alpha.Maintenance.Service.Interface.AdminTools
{
    using Couchbase;

    public interface CouchbaseFactory
    {
        ICouchbaseClient CreateClient(string bucket);
    }
}
