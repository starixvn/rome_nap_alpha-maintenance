namespace Alpha.Maintenance.Service.Interface.AdminTools
{
    using Enyim.Caching.Memcached.Results;
    using System.Collections.Generic;

    public interface CouchbaseService
    {
        IEnumerable<object> GetMultipleTransferKey(string keyList);

        object GetTransferKey(string key);

        object Get(string key);

        IStoreOperationResult SetTransferKey(string key, object value);

        bool RemoveMultipleTransferKey(string keyList);

        bool RemoveTransferKey(string key);

        IStoreOperationResult Set(string key, object value);

        bool Remove(string key);
    }
}
