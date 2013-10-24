using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Entities;
using CMS.Domain.Abstract;

namespace CMS.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IPageRepository PageRepository;

        public HomeController(IPageRepository PageRepo)
        {
            PageRepository = PageRepo;
        }

        public ActionResult Index(int id = 0)
        {
            if (id == 0)
            {
                return View("Home");
            }
            else
            {
                Page m_Page = PageRepository.RetrieveOne(id);
                return View(m_Page.TemplateName, m_Page);
            }
        }

    }
}
