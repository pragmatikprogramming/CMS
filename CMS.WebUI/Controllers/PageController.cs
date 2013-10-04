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

        
        public PageController(IPageRepository PageRepo)
        {
            PageRepository = PageRepo;
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
            m_Page.PageWorkFlowState = Utility.GetPageWorkFlowStatus(m_Page.Id);
            m_Page.LockedBy = Utility.GetLockedBy(m_Page.Id);

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

            if(!PageRepository.TrashCan(m_Page.PageID))
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

            return View(m_Page.TemplateName);
        }

        [CMSAuth]
        public ActionResult getContent(int id = 0)
        {
            Page m_Page = PageRepository.RetrieveOne(id);

            return View("getContent", m_Page);
        }
    }
}
