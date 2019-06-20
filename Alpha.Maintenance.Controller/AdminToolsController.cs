namespace Alpha.Maintenance.Controller
{
    using Alpha.Maintenance.Service.Impl.AdminTools;
    using Alpha.Maintenance.Service.Interface.AdminTools;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Linq;
    using System.Web.Mvc;

    [NapSuperAuth]
    public class AdminToolsController : BaseController
    {
        private readonly CouchbaseService _couchbaseService;


        public AdminToolsController() : this(null)
        {

        }
        public AdminToolsController(CouchbaseService couchbaseService)
        {
            _couchbaseService = couchbaseService ?? new CouchbaseServiceImpl();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Read)]
        public ActionResult CouchbaseManagement() => View();

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Read)]
        public JsonResult CouchbaseSelTransferDocument(string custIds)
           => Json(_couchbaseService.GetMultipleTransferKey(custIds).ToList());

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Host)]
        public JsonResult CouchbaseUpdDocument(string custId, string value)
            => Json(_couchbaseService.SetTransferKey(custId, value));

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Couchbase, Policy = Constant.Policy.Host)]
        public JsonResult CouchbaseDelTransferDocument(string custIds)
            => Json(_couchbaseService.RemoveMultipleTransferKey(custIds));
    }
}
