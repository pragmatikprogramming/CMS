using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Models;
using CMS.Domain.Entities;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class JSONController : Controller
    {
        IJSONRepository JSONRepository;

        public JSONController(IJSONRepository JSONRepo)
        {
            JSONRepository = JSONRepo;
        }

        [CMSAuth]
        public ActionResult getImages()
        {
            List<JSONImages> m_Images = JSONRepository.getImages();
            return this.Json(m_Images, JsonRequestBehavior.AllowGet);
        }

    }
}
