using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class HTMLWidgetController : Controller
    {
        IHTMLWidgetRepository HTMLWidgetRepository;

        public HTMLWidgetController(IHTMLWidgetRepository HTMLWidgetRepo)
        {
            HTMLWidgetRepository = HTMLWidgetRepo;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            List<HTMLWidget> m_Widgets = HTMLWidgetRepository.RetrieveAll();
            return View("Index", m_Widgets);            
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult WidgetAdd()
        {
            HTMLWidget m_Widget = new HTMLWidget();
            return View("WidgetAdd", m_Widget);
        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WidgetAdd(HTMLWidget m_Widget)
        {
            if (ModelState.IsValid)
            {
                HTMLWidgetRepository.Create(m_Widget);
                return RedirectToAction("Index", "HTMLWidget");
            }
            else
            {
                return View("WidgetAdd", m_Widget);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult WidgetEdit(int id)
        {
            HTMLWidget m_Widget = HTMLWidgetRepository.RetrieveOne(id);
            return View("WidgetEdit", m_Widget);
        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WidgetEdit(HTMLWidget m_Widget)
        {
            if (ModelState.IsValid)
            {
                HTMLWidgetRepository.Update(m_Widget);
                return RedirectToAction("Index", "HTMLWidget");
            }
            else
            {
                return View("WidgetEdit", m_Widget);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult WidgetDelete(int id)
        {
            HTMLWidgetRepository.Delete(id);
            return RedirectToAction("Index", "HTMLWidget");
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult Preview(int id)
        {
            ViewBag.Id = id;
            return View("WidgetPreview");
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult getWidget(int id)
        {
            HTMLWidget m_Widget = HTMLWidgetRepository.RetrieveOne(id);
            return View("getWidget", m_Widget);
        }
    }
}
