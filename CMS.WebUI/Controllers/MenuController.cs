using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Entities;
using CMS.Domain.Models;
using CMS.Domain.Abstract;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class MenuController : Controller
    {
        IMenuRepository MenuRepository;
        IMenuItemRepository MenuItemRepository;

        public MenuController(IMenuRepository MenuRepo, IMenuItemRepository MenuItemRepo)
        {
            MenuRepository = MenuRepo;
            MenuItemRepository = MenuItemRepo;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            List<Menu> m_Menus = MenuRepository.RetrieveAll();
            return View("Index", m_Menus);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddMenu()
        {
            Menu m_Menu = new Menu();
            return View("AddMenu", m_Menu);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult AddMenu(Menu m_Menu)
        {
            if (ModelState.IsValid)
            {
                MenuRepository.Create(m_Menu);
                return RedirectToAction("Index", "Menu");
            }
            else
            {
                return View("AddMenu", m_Menu);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditMenu(int id)
        {
            Menu m_Menu = MenuRepository.RetrieveOne(id);

            return View("EditMenu", m_Menu);
        }
        
        [CMSAuth]
        [HttpPost]
        public ActionResult EditMenu(Menu m_Menu)
        {
            if (ModelState.IsValid)
            {
                MenuRepository.Update(m_Menu);
                return RedirectToAction("Index", "Menu");
            }
            else
            {
                return View("EditMenu", m_Menu);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult DeleteMenu(int id)
        {
            MenuRepository.Delete(id);
            return RedirectToAction("Index", "Menu");
        }

        [CMSAuth]
        public ActionResult MenuItems(int id)
        {
            ViewBag.ParentId = id;
            List<MenuItem> m_MenuItems = MenuItemRepository.RetrieveAll(id);
            return View("MenuItem", m_MenuItems);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddMenuItem(int id)
        {
            ViewBag.ParentId = id;
            MenuItem m_MenuItem = new MenuItem();
            return View("AddMenuItem", m_MenuItem);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult AddMenuItem(MenuItem m_MenuItem)
        {
            if (ModelState.IsValid)
            {
                MenuItemRepository.Create(m_MenuItem);
                return RedirectToAction("MenuItems", "Menu", new { id = m_MenuItem.ParentId });
            }
            else
            {
                ViewBag.ParentId = m_MenuItem.ParentId;
                return View("AddMenuItem", m_MenuItem);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditMenuItem(int id)
        {
            MenuItem m_MenuItem = MenuItemRepository.RetrieveOne(id);
            return View("EditMenuItem", m_MenuItem);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult EditMenuItem(MenuItem m_MenuItem)
        {
            if (ModelState.IsValid)
            {
                MenuItemRepository.Update(m_MenuItem);
                return RedirectToAction("MenuItems", "Menu", new { id = m_MenuItem.ParentId });
            }
            else
            {
                return View("EditMenuItem", m_MenuItem);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult DeleteMenuItem(int id)
        {
            MenuItem m_MenuItem = MenuItemRepository.RetrieveOne(id);
            MenuItemRepository.Delete(id);
            return RedirectToAction("MenuItems", "Menu", new { id = m_MenuItem.ParentId });
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult sortUp(int id)
        {
            MenuItem m_MenuItem = MenuItemRepository.RetrieveOne(id);
            MenuItemRepository.sortUp(id);
            return RedirectToAction("MenuItems", "Menu", new { id = m_MenuItem.ParentId });
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult sortDown(int id)
        {
            MenuItem m_MenuItem = MenuItemRepository.RetrieveOne(id);
            MenuItemRepository.sortDown(id);
            return RedirectToAction("MenuItems", "Menu", new { id = m_MenuItem.ParentId });
        }

        [CMSAuth]
        public ActionResult getLinkUrl(int id)
        {
            ViewBag.myId = id;
            return View("getLinkUrl");
        }
        
    }
}
