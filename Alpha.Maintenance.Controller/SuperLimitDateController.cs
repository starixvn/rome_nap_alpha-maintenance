namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Web.Mvc;
    using Core.Model;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Repository.Criteria.SuperLimitDate;
    using Service;
    using ViewModels;

    [NapSuperAuth]
    public class SuperLimitDateController : BaseController
    {
        private readonly ISuperLimitDateService service;

        public SuperLimitDateController() : this(null)
        {

        }

        public SuperLimitDateController(ISuperLimitDateService service)
        {
            this.service = service ?? new SuperLimitDateService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SuperLimitDate, Policy = Constant.Policy.Read)]
        public ActionResult Index() => View();

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.SuperLimitDate, Policy = Constant.Policy.Read)]
        public ActionResult Get(GetAllSuperLimitDateCriteria criteria)
        {
            var supers = service.Get(criteria);

            return View(new SuperLimitDateViewModel(supers, criteria.TotalRecords, criteria.PageIndex, criteria.PageSize));
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SuperLimitDate, Policy = Constant.Policy.Host)]
        public ActionResult Add() => View(new SuperLimitDate { MinDate = DateTime.Now.AddDays(-33), LimitDays = 33 });

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.SuperLimitDate, Policy = Constant.Policy.Host)]
        public ActionResult Add(SuperLimitDate model)
        {
            if (model.TypeDate.Equals("LimitDays", StringComparison.InvariantCultureIgnoreCase) && !model.LimitDays.HasValue)
            {
                ModelState.AddModelError(Convert.ToString(201), "Limit Days is required");
            }
            if (model.TypeDate.Equals("MinDate", StringComparison.InvariantCultureIgnoreCase) && !model.MinDate.HasValue)
            {
                ModelState.AddModelError(Convert.ToString(202), "Min Date is required");
            }

            if (ModelState.IsValid)
            {
                var criteria = new AddSuperLimitDateCriteria
                {
                    CustName = model.UserName,
                    UserId = CurrentSession.UserId,
                    LimitDays = model.TypeDate.Equals("LimitDays", StringComparison.InvariantCultureIgnoreCase) ? model.LimitDays : null,
                    MinDate = model.TypeDate.Equals("MinDate", StringComparison.InvariantCultureIgnoreCase) ? model.MinDate : null
                };

                var result = service.Add(criteria);
                if (result.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(Convert.ToString(result.Code), Convert.ToString(result.Message));
            }

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.SuperLimitDate, Policy = Constant.Policy.Host)]
        public ActionResult Delete(DeleteSuperLimitDateCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            var result = service.Delete(criteria);

            return Json(result);
        }
    }
}