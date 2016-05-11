// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.8.30">Created</change>
// </history>

namespace Alpha.Maintenance.Site.App_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    /// <summary>
    /// Bundle config.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// Register bundles.
        /// For more information on Bundling
        /// <see cref="http://go.microsoft.com/fwlink/?LinkId=254725"/>
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            // css
            bundles.AddAndInclude(
                "~/css/common",
                "~/Assets/styles/vendors/icon/style.css",
                "~/Assets/styles/main.min.css");

            // js
            bundles.Add(new ScriptBundle("~/scripts/common").Include(
                "~/Assets/scripts/lib/msgbox/msgbox-0.0.1.js",
                "~/Assets/scripts/Maintenance.js"));

            bundles.Add(new ScriptBundle("~/scripts/recheckin").Include(
             "~/Assets/Areas/UserLogin/scripts/ReCheckIn.js"));


            //// Log
            bundles.AddAndInclude(
                    "~/css/select2",
                    "~/Assets/scripts/lib/Select2/select2.css");

            bundles.Add(new ScriptBundle("~/scripts/log-index").Include(
               "~/Assets/scripts/lib/jquery-ui.min.js",
               "~/Assets/scripts/lib/Fanex.Grid/Fanex.Pagination.js",
               "~/Assets/scripts/lib/Momentjs/moment.min.js",
               "~/Assets/scripts/lib/Select2/select2.js",
               "~/Assets/scripts/Logs/Index.js"));
        }
    }
}