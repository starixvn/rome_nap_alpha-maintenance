namespace Alpha.Maintenance.Site.Areas
{
    using System.Web.Mvc;

    public class DatabaseProfilerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DatabaseProfiler";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + "_DefaultAreas",
                "DBProfiler/{controller}/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional });
        }
    }
}