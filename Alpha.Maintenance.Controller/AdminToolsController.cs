namespace Alpha.Maintenance.Controller
{
    using Alpha.Maintenance.Controller.Models.AdminTools;
    using Alpha.Maintenance.Service.Impl.AdminTools;
    using Alpha.Maintenance.Service.Interface.AdminTools;
    using Couchbase;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    [NapSuperAuth]
    public class AdminToolsController : BaseController
    {
        private readonly CouchbaseFactory couchbaseFactory;

        public AdminToolsController() : this(null)
        {
        }

        public AdminToolsController(CouchbaseFactory couchbaseFactory)
        {
            this.couchbaseFactory = couchbaseFactory ?? new CouchbaseFactoryImpl();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Read)]
        public ActionResult CouchbaseManagement() => View();

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Read)]
        public JsonResult CouchbaseSel(CouchbaseManagementModel model)
        {
            var couchbaseClient = couchbaseFactory.CreateClient("Alpha");
            var caches = model.Keys.Select(key =>
            {
                var cacheKey = GenerateCacheKey(model.Pattern, key);
                var cacheValue = couchbaseClient.Get(cacheKey);
                return new
                {
                    cacheKey,
                    cacheValue
                };
            });
            return Json(caches);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Host)]
        public JsonResult CouchbaseDel(CouchbaseManagementModel model)
        {
            var result = true;
            var couchbaseClient = couchbaseFactory.CreateClient("Alpha");
            model.Keys.ForEach(key =>
            {
                var cacheKey = GenerateCacheKey(model.Pattern, key);
                result = couchbaseClient.Remove(cacheKey) && result;
            });

            return Json(result);
        }

        private static string GenerateCacheKey(CacheKeyPattern pattern, string key)
        {
            switch (pattern)
            {
                case CacheKeyPattern.AgentMultipleTransfer:
                    return "AgentMultipleTransfer_" + key;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
