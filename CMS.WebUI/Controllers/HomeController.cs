using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Entities;
using CMS.Domain.Abstract;
using CMS.Domain.HelperClasses;


namespace CMS.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IPageRepository PageRepository;
        IHomeRepository HomeRepository;
        IImageRepository ImageRepository;
        IFormRepository FormRepository;
        IFAQRepository FAQRepository;

        public HomeController()
        {

        }

        public HomeController(IPageRepository PageRepo, IHomeRepository HomeRepo, IImageRepository ImageRepo, IFormRepository FormRepo, IFAQRepository FAQRepo)
        {
            PageRepository = PageRepo;
            HomeRepository = HomeRepo;
            ImageRepository = ImageRepo;
            FormRepository = FormRepo;
            FAQRepository = FAQRepo;
        }

        public ActionResult Index(int id = 0)
        {
            ViewBag.PageId = id;

            if (id == 0)
            {
                return View("Home");
            }
            else
            {
                Page m_Page = PageRepository.RetrieveOne(id);

                if (m_Page.RedirectURL == null || m_Page.RedirectURL == string.Empty)
                {
                    ViewBag.PageType = m_Page.PageType;
                    ViewBag.id = m_Page.PageTypeId;
                    return View(m_Page.TemplateName, m_Page);
                }
                else
                {
                    return Redirect(m_Page.RedirectURL);
                }
            }
        }

        public ActionResult MainMenu()
        {
            List<Page> m_Pages = HomeRepository.MainMenu();
            return View("MainMenu", m_Pages);
        }

        public ActionResult Container(int id)
        {
            WidgetContainer m_Container = HomeRepository.getContainer(id);
            return View("Container", m_Container);
        }

        public ActionResult NonSystemMenu(int id, string viewName)
        {
            List<MenuItem> m_MenuItems = HomeRepository.NonSystemMenu(id);
            ViewBag.MenuName = Utility.getMenuName(id);
            return View(viewName, m_MenuItems);
        }

        public ActionResult SystemSubMenu(int id)
        {
            List<Page> m_Pages = PageRepository.RetrieveAll(id);
            Page m_Page = PageRepository.RetrieveOne(id);
            ViewBag.ParentName = m_Page.NavigationName;

            return View("SystemSubMenu", m_Pages);
        }

        public ActionResult FeaturedEvents()
        {
            List<Event> m_Events = HomeRepository.FeaturedEvents();
            return View("FeaturedEvents", m_Events);
        }

        public ActionResult getTeenBlog()
        {
            ViewBag.Count = 0;
            List<BlogPost> m_BlogPosts = HomeRepository.GetTeenBlog();
            return View("getTeenBlog", m_BlogPosts);
        }

        public ActionResult getNews()
        {
            ViewBag.Count = 1;
            List<BlogPost> m_BlogPosts = HomeRepository.GetNews();
            return View("getNews", m_BlogPosts);
        }

        public ActionResult getForm(int id)
        {
            Form m_Form = FormRepository.RetrieveOne(id);
            return View("getForm", m_Form);
        }

        public ActionResult getFAQ(int id)
        {
            List<FAQQuestions> m_Questions = FAQRepository.RetrieveAllFAQQuestions(id);
            return View("getFAQ", m_Questions);
        }

        public ActionResult SwapNews(int id)
        {
            ViewBag.BlogPost = HomeRepository.SwapNews(id);
            ViewBag.Id = id;
            ViewBag.Count = 1;
            List<BlogPost> m_BlogPosts = HomeRepository.GetNews();

            return View("SwapNews", m_BlogPosts);
        }
    }
}
