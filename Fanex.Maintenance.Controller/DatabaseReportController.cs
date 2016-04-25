// --------------------------------------------------------------------------
// <copyright file="DatabaseReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.10.16">StyleCop</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.DataValidation.Service.Implement;
    using Fanex.DataValidation.Validation.Implement;
    using Fanex.Maintenance.Repository.Criteria.DatabaseReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Database report controller.
    /// </summary>
    [NapSuperAuth]
    public class DatabaseReportController : BaseController
    {
        /// <summary>
        /// Data Integrity Return view.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.DataIntegrity, Policy = Constant.Policy.Read)]
        public ActionResult DataIntegrity()
        {
            var validationService = new ValidationService();
            DateTime moveBetsDay = validationService.MoveBetsDay;

            ViewBag.MaxDate = string.Format("{0:dd/MM/yyyy}", moveBetsDay);
            ViewBag.BodyLicenseeData = string.Empty;
            ViewBag.BodyCreditData = string.Empty;

            var listDrawFilter = new List<SelectListItem>();

            listDrawFilter.Add(new SelectListItem
            {
                Text = "Normal",
                Value = "1"
            });
            listDrawFilter.Add(new SelectListItem
            {
                Text = "No Draw",
                Value = "2"
            });
            listDrawFilter.Add(new SelectListItem
            {
                Text = "No Draw for Turnover Only",
                Value = "3"
            });

            ViewBag.ListDrawFilter = listDrawFilter;

            return View(new DataIntegrityViewModel
            {
                Date = moveBetsDay.ToString("dd/MM/yyyy")
            });
        }

        #region Ajax

        /// <summary>
        /// Get deposit integrity.
        /// </summary>
        /// <param name="filter">Draw filter.</param>
        /// <param name="date">Date string.</param>
        /// <returns>Result string.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DataIntegrity, Policy = Constant.Policy.Read)]
        public string GetDepositIntegrity(int filter = 1, string date = "")
        {
            if (Request.IsAjaxRequest())
            {
                DateTime fromData = DateTime.Now;
                if (!string.IsNullOrEmpty(date))
                {
                    fromData = DateTime.Parse(date, new CultureInfo("en-GB", false));
                }

                bool isConsistent;
                var validationLicensee = new DepositValidation { DrawFilter = filter };

                return validationLicensee.GetDataComparisionString(fromData, fromData, false, out isConsistent);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get credit integrity.
        /// </summary>
        /// <param name="filter">Draw filter.</param>
        /// <param name="date">Date string.</param>
        /// <returns>Result string.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DataIntegrity, Policy = Constant.Policy.Read)]
        public string GetCreditIntegrity(int filter = 1, string date = "")
        {
            if (Request.IsAjaxRequest())
            {
                var fromData = DateTime.Now;
                if (!string.IsNullOrEmpty(date))
                {
                    fromData = DateTime.Parse(date, new CultureInfo("en-GB", false));
                }

                bool isConsistent;
                var validationCredit = new CreditValidation { DrawFilter = filter };

                return validationCredit.GetDataComparisionString(fromData, fromData, false, out isConsistent);
            }

            return string.Empty;
        }

        #endregion Ajax
    }
}