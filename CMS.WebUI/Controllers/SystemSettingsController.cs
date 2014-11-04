using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.WebUI.Infrastructure;
using CMS.Domain.Abstract;
using CMS.Domain.Models;
using CMS.Domain.Entities;

namespace CMS.WebUI.Controllers
{
    public class SystemSettingsController : Controller
    {
        //
        // GET: /SystemSettings/

        private ISystemSettingsRepository SystemSettingsRepository;
        private IImageRepository ImageRepository;

        public SystemSettingsController(ISystemSettingsRepository SystemSettingsRepo, IImageRepository ImageRepo)
        {
            SystemSettingsRepository = SystemSettingsRepo;
            ImageRepository = ImageRepo;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            SystemSettings m_Settings = SystemSettingsRepository.GetSystemSettings();

            return View("Index", m_Settings);
        }

        [CMSAuth]
        public ActionResult SaveSettings(SystemSettings m_Settings, HttpPostedFileBase fileUpload)
        {
            m_Settings.ImageBinary = ImageRepository.ToBinary(fileUpload);
            SystemSettingsRepository.UpdateSystemSettings(m_Settings);

            return Redirect("/Admin/Index");
        }

    }
}
