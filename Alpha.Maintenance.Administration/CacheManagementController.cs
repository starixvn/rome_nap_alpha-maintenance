namespace Alpha.Maintenance.Administration
{
    using Alpha.Maintenance.Administration.Models;
    using Alpha.Maintenance.Administration.Services;
    using Alpha.Maintenance.Controller;
    using Alpha.Maintenance.Controller.Models.AdminTools;
    using Fanex.Caching;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class CacheManagementController : BaseController
    {
        private readonly IRedisCacheFactory cacheFactory;

        public CacheManagementController()
            : this(null)
        {
        }

        public CacheManagementController(IRedisCacheFactory cacheFactory)
        {
            this.cacheFactory = cacheFactory ?? RedisCacheFactory.DefaultInstance;
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Read)]
        public ActionResult Index() => View();

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Host)]
        public async Task<JsonResult> Get(CacheManagementModel model)
        {
            var result = new List<object>();
            var cacheService = GetCacheService(model.Pattern);
            foreach (var key in model.Keys)
            {
                var cacheKey = GenerateCacheKey(model.Pattern, key);
                var cacheValue = await cacheService.GetAsync<object>(cacheKey);
                result.Add(new
                {
                    cacheKey,
                    cacheValue
                });
            }

            return Json(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Host)]
        public async Task Delete(CacheManagementModel model)
        {
            var cacheService = GetCacheService(model.Pattern);
            foreach (var key in model.Keys)
            {
                var cacheKey = GenerateCacheKey(model.Pattern, key);
                await cacheService.RemoveAsync(cacheKey);
            }
        }

        private ICacheService GetCacheService(CacheKeyPattern pattern)
        {
            switch (pattern)
            {
                case CacheKeyPattern.LoginBlockedIP:
                    return cacheFactory.Create("RedisConfigurationOptions:Alpha.Before");

                default:
                    throw new NotImplementedException();
            }
        }

        private static string GenerateCacheKey(CacheKeyPattern pattern, string key)
        {
            switch (pattern)
            {
                case CacheKeyPattern.LoginBlockedIP:
                    return "Fanex.Alpha.Before.Common.Constants.SessionConstants.totalloginfailedcount." + key;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
