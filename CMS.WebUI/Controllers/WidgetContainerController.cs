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
    public class WidgetContainerController : Controller
    {
        IHTMLWidgetRepository HTMLWidgetRepository;
        IWidgetContainer WidgetContainerRepository;

        public WidgetContainerController(IWidgetContainer WidgetContainerRepo, IHTMLWidgetRepository HTMLWidgetRepo)
        {
            HTMLWidgetRepository = HTMLWidgetRepo;
            WidgetContainerRepository = WidgetContainerRepo;
        }
        
        [CMSAuth]
        public ActionResult Index()
        {
            List<WidgetContainer> m_Containers = WidgetContainerRepository.RetrieveAll();
            return View("Index", m_Containers);
        }


        [HttpGet]
        [CMSAuth]
        public ActionResult ContainerAdd()
        {
            WidgetContainer m_Container = new WidgetContainer();
            ViewBag.Templates = Utility.GetTemplates();
            ViewBag.Widgets = WidgetContainerRepository.getWidgets();

            if (ViewBag.Widgets == null)
            {
                ViewBag.Widgets = new List<HTMLWidget>();
            }

            return View("ContainerAdd", m_Container);
        }

        [HttpPost]
        [CMSAuth]
        public ActionResult ContainerAdd(WidgetContainer m_Container)
        {
            
            if (ModelState.IsValid)
            {
                int containerId = WidgetContainerRepository.Create(m_Container);
                return RedirectToAction("Index", "WidgetContainer");
            }
            else
            {
                ViewBag.Widgets = WidgetContainerRepository.getWidgets();
                return View("ContainerAdd", m_Container);
            }
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult ContainerEdit(int id)
        {
            ViewBag.Widgets = WidgetContainerRepository.getWidgets();
            WidgetContainer m_Container = WidgetContainerRepository.RetrieveOne(id);
            return View("ContainerEdit", m_Container);
        }

        [HttpPost]
        [CMSAuth]
        public ActionResult ContainerEdit(WidgetContainer m_Container)
        {
            if (ModelState.IsValid)
            {
                WidgetContainerRepository.Update(m_Container);
                return RedirectToAction("Index", "WidgetContainer");
            }
            else
            {
                if (m_Container.MyWidgets == null)
                {
                    m_Container.MyWidgets = new List<int>();
                }
                ViewBag.Widgets = WidgetContainerRepository.getWidgets();
                return View("ContainerEdit", m_Container);
            }
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult ContainerDelete(int id)
        {
            WidgetContainerRepository.Delete(id);
            return RedirectToAction("Index", "WidgetContainer");
        }

        [HttpGet]
        public ActionResult OrderWidgets(int id)
        {
            ViewBag.ContainerId = id;
            List<HTMLWidget> m_Widgets = WidgetContainerRepository.RetrieveAll(id);
            return View("OrderWidgets", m_Widgets);
        }

        [HttpGet]
        public ActionResult SortUp(int parentId, int id)
        {
            WidgetContainerRepository.SortUp(parentId, id);
            return RedirectToAction("OrderWidgets", "WidgetContainer", new { id = parentId });
        }

        [HttpGet]
        public ActionResult SortDown(int parentId, int id)
        {
            WidgetContainerRepository.SortDown(parentId, id);
            return RedirectToAction("OrderWidgets", "WidgetContainer", new { id = parentId });
        }

        [HttpGet]
        public ActionResult Preview(int id)
        {
            ViewBag.Id = id;
            return View("WidgetPreview");
        }

        [HttpGet]
        public ActionResult getContainer(int id)
        {
            List<HTMLWidget> m_Widgets = WidgetContainerRepository.RetrieveAll(id);
            return View("getContainer", m_Widgets);
        }
    }
}
