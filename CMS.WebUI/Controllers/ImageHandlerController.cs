using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class ImageHandlerController : Controller
    {
        IImageRepository ImageRepository;
        ISystemSettingsRepository SystemSettingsRepository;

        public ImageHandlerController(IImageRepository ImageRepo, ISystemSettingsRepository SystemSettingsRepo)
        {
            ImageRepository = ImageRepo;
            SystemSettingsRepository = SystemSettingsRepo;
        }

        [CMSAuth]
        public ActionResult GetAdminLogo()
        {
            SystemSettings m_Settings = SystemSettingsRepository.GetSystemSettings();
            return File(m_Settings.ImageBinary, "image/png");
        }

    }
}
