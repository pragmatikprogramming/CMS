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
        IBlogPostRepository BlogPostRepository;

        public HomeController()
        {

        }

        public HomeController(IPageRepository PageRepo, IHomeRepository HomeRepo, IImageRepository ImageRepo, IFormRepository FormRepo, IFAQRepository FAQRepo, IBlogPostRepository BlogPostRepo)
        {
            PageRepository = PageRepo;
            HomeRepository = HomeRepo;
            ImageRepository = ImageRepo;
            FormRepository = FormRepo;
            FAQRepository = FAQRepo;
            BlogPostRepository = BlogPostRepo;
        }

        public ActionResult Index(int id = 0)
        {
            if (id == 0)
            {
                Page m_Page = PageRepository.RetrieveOne(39);
                ViewBag.TemplateId = 4;
                return View("Home", m_Page);
            }
            else
            {
                Page m_Page = PageRepository.RetrieveOne(id);

                if (m_Page.RedirectURL == null || m_Page.RedirectURL == string.Empty)
                {
                    ViewBag.PageType = m_Page.PageType;
                    ViewBag.id = m_Page.PageTypeId;
                    ViewBag.PageId = m_Page.PageID;
                    ViewBag.TemplateId = m_Page.TemplateId;
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

        public ActionResult getBlog(int parentId, int id)
        {
            ViewBag.Count = 0;
            ViewBag.TemplateId = parentId;
            List<BlogPost> m_BlogPosts = HomeRepository.GetBlog(id);
            return View("getBlog", m_BlogPosts);
        }

        public ActionResult getNews()
        {
            ViewBag.Count = 1;
            List<BlogPost> m_BlogPosts = HomeRepository.GetNews();
            ViewBag.NewsId = m_BlogPosts[0].Id;
            return View("getNews", m_BlogPosts);
        }

        public ActionResult getForm(int parentId, int id)
        {
            ViewBag.ParentId = parentId;
            ViewBag.Count = 0;
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

        public ActionResult BlogPost(int id, int parentId = 5)
        {
            BlogPost m_BlogPost = BlogPostRepository.RetrieveOne(id);
            ViewBag.PageType = 5;
            ViewBag.PageId = null;
            ViewBag.TemplateId = parentId;
            string m_Template = Utility.GetTemplateById(parentId);

            return View(m_Template, m_BlogPost);
        }


        [HttpPost]
        public ActionResult SubmitComment(BlogPostComment m_Comment)
        {
            HomeRepository.SubmitComment(m_Comment);
            return RedirectToAction("BlogPost", "Home", new { id = m_Comment.Id });
        }

        public ActionResult GetComments(int id)
        {
            List<BlogPostComment> m_Comments = BlogPostRepository.GetComments(id);
            return View("GetComments", m_Comments);
        }

        [HttpPost]
        public ActionResult ProcessForm(int parentId, int id)
        {
            string formData = "";
            string emailBody = "";
            Form m_Form = FormRepository.RetrieveOne(id);

            Dictionary<string, int> m_Ffs = new Dictionary<string,int>();

            foreach (FormField ff in m_Form.FormFields)
            {
                if (ff.IsRequired == 1)
                {
                    m_Ffs.Add(ff.Label, 1);
                }
                else
                {
                    m_Ffs.Add(ff.Label, 0);
                }
            }

            foreach (string key in Request.Form.Keys)
            {
                if (string.IsNullOrEmpty(Request.Form[key]) && m_Ffs[key] == 1)
                {
                    ModelState.AddModelError(key, "Please enter a value for " + key);
                }
                formData += key + "::" + Request.Form[key] + "^^";
                emailBody += key + " = " + Request.Form[key] + "<br />";
            }

            if (ModelState.IsValid)
            {
                FormRepository.InsertFormData(formData, id);

                Page m_Page = PageRepository.RetrieveOne(parentId);
                FormRepository.SendFormData(m_Form.SubmissionEmail, "webmaster@solanolibrary.com", emailBody, m_Form.FormName + " - Submission");

                ViewBag.PageType = m_Page.PageType;
                ViewBag.id = m_Page.PageTypeId;
                ViewBag.PageId = m_Page.TemplateId;
                ViewBag.Message = "Your information has been submitted.";
                return View(m_Page.TemplateName, m_Page);
            }
            else
            {
                Page m_Page = PageRepository.RetrieveOne(parentId);
                ViewBag.PageType = m_Page.PageType;
                ViewBag.id = m_Page.PageTypeId;
                ViewBag.PageId = m_Page.TemplateId;
                return View(m_Page.TemplateName, m_Page);
                //
                //return View("getForm", m_Form);
            }
        }
    }
}
