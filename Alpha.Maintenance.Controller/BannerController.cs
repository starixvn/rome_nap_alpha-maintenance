namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using Alpha.Maintenance.Controller.ViewModels;
    using Alpha.Maintenance.Core;
    using Alpha.Maintenance.Repository.Criteria.Banners;
    using Alpha.Maintenance.Service.Impl.Banners;
    using Alpha.Maintenance.Service.Interface.Banners;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapSuperAuth]
    public class BannerController : BaseController
    {
        private readonly BannerService bannerService;
        private const int DaysOfWeek = 7;

        public BannerController()
            : this(null)
        {
        }

        internal BannerController(BannerService bannerService)
            => this.bannerService = bannerService ?? new BannerServiceImpl();

        [NapAuth(FunctionKey = SiteFunctionKeys.BannerManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index(ViewBannerViewModel viewModel)
        {
            viewModel.Banners = bannerService.GetBanners(
                viewModel.FilterBy,
                viewModel.FromDate,
                viewModel.ToDate,
                viewModel.Environment,
                viewModel.Site);
            viewModel.HasHostPermission = HasRight(SiteFunctionKeys.BannerManagement, Constant.Policy.Host);

            return View(viewModel);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.BannerManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add()
        {
            var today = DateTime.Today;

            return View(
                "~/Views/Banner/UploadBanner.cshtml",
                new UploadBannerViewModel(
                    HasRight(SiteFunctionKeys.BannerManagement, Constant.Policy.Host),
                    ViewResource.AddBanner)
                    {
                        ReleasedDate = today,
                        ExpirationDate = today.AddDays(DaysOfWeek)
                    });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.BannerManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(UploadBannerViewModel viewModel)
        {
            var request = new AddBannerCriteria
            {
                BannerName = viewModel.Title,
                IsActive = viewModel.IsActive,
                ReleasedDate = viewModel.ReleasedDate,
                ExpirationDate = viewModel.ExpirationDate,
                Environments = viewModel.Environments,
                Systems = viewModel.Sites,
                Creator = CurrentSession.UserId
            };

            return Json(bannerService.AddBanner(request,ToHashtable(Request.Files)),JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.BannerManagement, Policy = Constant.Policy.Read)]
        public ActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var bannerForEdit = bannerService.GetBanner(id.Value);
            var banner = bannerForEdit.Item1;

            if (banner == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(
                "~/Views/Banner/UploadBanner.cshtml",
                new UploadBannerViewModel(
                    HasRight(SiteFunctionKeys.BannerManagement, Constant.Policy.Host),
                    ViewResource.EditBanner,
                    bannerForEdit.Item2,
                    bannerForEdit.Item3)
                    {
                        Id = banner.Id,
                        Title = banner.Title,
                        IsActive = banner.IsActive,
                        ReleasedDate = banner.ReleasedDate,
                        ExpirationDate = banner.ExpirationDate,
                        Environments = banner.Environments,
                        Sites = banner.Sites
                    });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.BannerManagement, Policy = Constant.Policy.Host)]
        public ActionResult Edit(UploadBannerViewModel viewModel)
        {
            var request = new EditBannerCriteria
            {
                BannerId = viewModel.Id,
                BannerName = viewModel.Title,
                IsActive = viewModel.IsActive,
                ReleasedDate = viewModel.ReleasedDate,
                ExpirationDate = viewModel.ExpirationDate,
                Environments = viewModel.Environments,
                Systems = viewModel.Sites,
                Creator = CurrentSession.UserId
            };
            return Json(
                bannerService.EditBanner(
                    request,
                    ToHashtable(Request.Files)),
                JsonRequestBehavior.DenyGet);
        }

        private static Hashtable ToHashtable(HttpFileCollectionBase files)
        {
            var banners = new Hashtable(StringComparer.OrdinalIgnoreCase);

            foreach (string key in files)
            {
                banners[key] = ToByteArray(files[key]);
            }

            return banners;
        }

        private static byte[] ToByteArray(HttpPostedFileBase file)
        {
            byte[] buffer;

            using (var inputStream = file.InputStream)
            {
                var target = new MemoryStream();
                inputStream.CopyTo(target);
                buffer = target.ToArray();
            }

            return buffer;
        }
    }
}
