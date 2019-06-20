namespace Alpha.Maintenance.Service.Impl.AdminTools
{
    using Alpha.Maintenance.Core.Configurations;
    using Alpha.Maintenance.Service.Interface.AdminTools;
    using Couchbase;
    using Enyim.Caching.Memcached;
    using Enyim.Caching.Memcached.Results;
    using System;
    using System.Collections.Generic;

    public class CouchbaseServiceImpl : CouchbaseService
    {
        private ICouchbaseClient _couchbaseClient;

        private const string _agentTransferPrefix = "AgentMultipleTransfer_";

        public CouchbaseServiceImpl() : this(null)
        { }

        public CouchbaseServiceImpl(ICouchbaseClient couchbaseClient)
        {
            _couchbaseClient = couchbaseClient ?? new CouchbaseClient();
        }

        public IEnumerable<object> GetMultipleTransferKey(string keyList)
        {
            var arrKey = keyList.Trim().Split(',');
            foreach (var key in arrKey)
            {
                yield return Get(_agentTransferPrefix + key.Trim());
            }
        }

        public object GetTransferKey(string key) => Get(_agentTransferPrefix + key.Trim());

        public IStoreOperationResult SetTransferKey(string key, object value) => Set(_agentTransferPrefix + key.Trim(), value);

        public bool RemoveMultipleTransferKey(string keyList)
        {
            bool result = false;
            var arrKey = keyList.Trim().Split(',');
            foreach (var key in arrKey)
            {
                result = result || RemoveTransferKey(key.Trim());
            }

            return result;
        }

        public bool RemoveTransferKey(string key) => Remove(_agentTransferPrefix + key.Trim());

        public object Get(string key) => _couchbaseClient.Get(key.Trim());

        public IStoreOperationResult Set(string key, object value) =>
            _couchbaseClient.ExecuteStore(StoreMode.Set, key.Trim(), value, new TimeSpan(0, Convert.ToInt32(AppSettings.GetConfigValue("MultipleTransferCacheTimeOut")), 0));

        public bool Remove(string key) => _couchbaseClient.Remove(key.Trim());
    }
}
