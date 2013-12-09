using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using CMS.WebUI.Infrastructure;
using CMS.Domain.DataAccess;

namespace CMS.WebUI.Controllers
{
    public class PageController : Controller
    {
        IPageRepository PageRepository;
        IJSONRepository JSONRepository;
        IFormRepository FormRepository;
        IFAQRepository FAQRepository;

        
        public PageController(IPageRepository PageRepo, IJSONRepository JSONRepo, IFormRepository FormRepo, IFAQRepository FAQRepo)
        {
            PageRepository = PageRepo;
            JSONRepository = JSONRepo;
            FormRepository = FormRepo;
            FAQRepository = FAQRepo;
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult Index(int id = 0)
        {
            ViewBag.ParentId = id;
            ViewBag.m_SessionId = (int)System.Web.HttpContext.Current.Session["uid"];

            List<Page> m_Pages = PageRepository.RetrieveAll(id);

            return View("Pages", m_Pages);
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult AddPage(int id = 0)
        {
            ViewBag.ParentId = id;
            ViewBag.myContentGroups = Utility.ContentGroups();
            ViewBag.Templates = Utility.GetTemplates();

            Page m_Page = new Page();

            return View("AddPage", m_Page);
        }

        [HttpPost]
        [CMSAuth]
        [ValidateInput(false)]
        public ActionResult AddPage(Page m_Page)
        {
            if (m_Page.ParentId != 0)
            {
                var ExpireDate = ModelState["ExpireDate"];
                ExpireDate.Errors.Clear();
            }

            if (m_Page.ParentId > 0)
            {
                if (DateTime.Compare(m_Page.PublishDate, m_Page.ExpireDate) > 0 && m_Page.ExpireDate != DateTime.MinValue)
                {
                    ModelState.AddModelError("PublishDate", "Invalid publish date. The date select is not before the expire date");
                }
            }
            else
            {
                if (m_Page.ExpireDate != DateTime.MinValue)
                {
                    ModelState.AddModelError("ExpireDate", "You cannot set an expiration date to a top level page");
                }
            }

            if (ModelState.IsValid)
            {
                m_Page.PageSetDefaults();
                PageRepository.Create(m_Page);
                return RedirectToAction("Index", "Page", new { id = m_Page.ParentId });
            }
            else
            {
                ViewBag.ParentId = m_Page.ParentId;
                ViewBag.myContentGroups = Utility.ContentGroups();
                ViewBag.Templates = Utility.GetTemplates();

                return View("AddPage", m_Page);
            }
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult EditPage(int id = 0)
        {
            Page m_Page = PageRepository.RetrieveOne(id);

            if (m_Page.LockedBy > 0 && m_Page.LockedBy != (int)System.Web.HttpContext.Current.Session["uid"])
            {
                ModelState.AddModelError("LockedBy", "The page is currently locked");
                ViewBag.ParentId = m_Page.ParentId;
                ViewBag.m_SessionId = (int)System.Web.HttpContext.Current.Session["uid"];
                List<Page> m_Pages = PageRepository.RetrieveAll(m_Page.ParentId);

                return View("Pages", m_Pages);
            }
            else
            {
                PageRepository.LockPage(id);
                ViewBag.myContentGroups = Utility.ContentGroups();
                ViewBag.Templates = Utility.GetTemplates();
                return View("EditPage", m_Page);
            }
        }

        [HttpPost]
        [CMSAuth]
        [ValidateInput(false)]
        public ActionResult EditPage(Page m_Page)
        {
            

            if (m_Page.ParentId > 0)
            {
                var ExpireDate = ModelState["ExpireDate"];
                ExpireDate.Errors.Clear();
            }
            if (m_Page.ParentId > 0)
            {
                if (DateTime.Compare(m_Page.PublishDate, m_Page.ExpireDate) > 0 && m_Page.ExpireDate != DateTime.MinValue)
                {
                    ModelState.AddModelError("PublishDate", "Invalid publish date. The date select is not before the expire date");
                }
            }
            else
            {
                if (m_Page.ExpireDate != DateTime.MinValue)
                {
                    ModelState.AddModelError("ExpireDate", "You cannot set an expiration date to a top level page");
                }
            }

            if (Utility.GetLockedBy(m_Page.Id) != (int)System.Web.HttpContext.Current.Session["uid"])
            {
                return RedirectToAction("Index", "Page", new { id = m_Page.ParentId });
            }

            if (ModelState.IsValid)
            {
                m_Page.PageSetDefaults();
                PageRepository.Update(m_Page);
                return RedirectToAction("Index", "Page", new { id = m_Page.ParentId });
            }
            else
            {
                ViewBag.ParentId = m_Page.ParentId;
                ViewBag.myContentGroups = Utility.ContentGroups();
                ViewBag.Templates = Utility.GetTemplates();

                return View("EditPage", m_Page);
            }
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult PagePublish(int id = 0)
        {
            int ParentId = PageRepository.Publish(id);
            PageRepository.UnlockPage(id);
            return RedirectToAction("Index", "Page", new { id = ParentId });
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult PageDelete(int id = 0)
        {
            Page m_Page = PageRepository.RetrieveOne(id);

            if(!PageRepository.TrashCan(m_Page.Id))
            {
                ModelState.AddModelError("ParentId", "This page has subchildren. Delete all childred before deleting this page!");
            }


            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Page", new { id = m_Page.ParentId });
            }
            else
            {
                ViewBag.ParentId = m_Page.ParentId;
                List<Page> m_Pages = PageRepository.RetrieveAll(m_Page.ParentId);
                return View("Pages", m_Pages);
            }
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult PageUnlock(int id = 0)
        {
            Page m_Page = PageRepository.RetrieveOne(id);

            if (id > 0)
            {
                PageRepository.UnlockPage(id);
            }

            return RedirectToAction("Index", "Page", new { id = m_Page.ParentId });
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult PagePreview(int id = 0)
        {
            Page m_Page = PageRepository.RetrieveOne(id);
            ViewBag.Content = m_Page.Content;

            return View(m_Page.TemplateName, m_Page);
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult OrderUp(int id)
        {
            Page m_Page = PageRepository.RetrieveOne(id);
            PageRepository.sortUp(id);
            return RedirectToAction("Index", "Page", new { id = m_Page.ParentId });
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult OrderDown(int id)
        {
            Page m_Page = PageRepository.RetrieveOne(id);
            PageRepository.sortDown(id);
            return RedirectToAction("Index", "Page", new { id = m_Page.ParentId });
        }

        [HttpPost]
        [CMSAuth]
        public ActionResult getPageTypeIds(int mPageType)
        {
            ViewBag.PageType = mPageType;

            if (mPageType == 2)
            {
                List<Form> m_Forms = FormRepository.RetrieveAll();
                return View("getForms", m_Forms);
            }
            else if (mPageType == 3)
            {
                List<FAQ> m_FAQ = FAQRepository.RetrieveAllFAQ();
                return View("getFAQ", m_FAQ);
            }
            else
            {
                ViewBag.Templates = Utility.GetTemplates();
                return View("getDefaults");
            }
        }
    }
}
