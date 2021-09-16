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
    using System.Configuration;
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

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Read)]
        public ActionResult HealthCheck()
        {
            var cacheKeyPattern = CacheKeyPattern.LoginBlockedIP;
            var redisSettingKey = GetRedisSettingKey(cacheKeyPattern);
            var redisConnection = ConfigurationManager.AppSettings[redisSettingKey];
            var cacheService = GetCacheService(cacheKeyPattern);
            var model = new HealthCheckModel(redisConnection);
            try
            {
                cacheService.Contain("TestKey");
            }
            catch (Exception ex)
            {
                model = new HealthCheckModel(redisConnection, ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Read)]
        public async Task<JsonResult> Get(CacheManagementModel model)
        {
            var result = new List<object>();
            var cacheService = GetCacheService(model.Pattern);
            foreach (var key in model.Keys)
            {
                var cacheKey = GenerateCacheKey(model, key);
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
                var cacheKey = GenerateCacheKey(model, key);
                await cacheService.RemoveAsync(cacheKey);
            }
        }

        private ICacheService GetCacheService(CacheKeyPattern pattern)
        {
            return cacheFactory.Create(GetRedisSettingKey(pattern));
        }

        private static string GetRedisSettingKey(CacheKeyPattern pattern)
        {
            switch (pattern)
            {
                case CacheKeyPattern.AgentMultipleTransfer:
                    return "RedisConfigurationOptions:Alpha.Transfer";

                case CacheKeyPattern.LoginBlockedIP:
                    return "RedisConfigurationOptions:Alpha.Before";

                default:
                    throw new NotImplementedException();
            }
        }

        private static string GenerateCacheKey(CacheManagementModel model, string key)
        {
            switch (model.Pattern)
            {
                case CacheKeyPattern.AgentMultipleTransfer:
                    return $"AgentMultipleTransfer.{model.SiteGroup}.{key}";

                case CacheKeyPattern.LoginBlockedIP:
                    return $"loginipblocked.{model.SiteGroup}.{key}";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
