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
    public class CategoryController : Controller
    {
        ICategoryRepository CategoryRepository;

        public CategoryController(ICategoryRepository CategoryRepo)
        {
            CategoryRepository = CategoryRepo;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            List<Category> m_Categories = CategoryRepository.RetrieveAll();

            return View("Index", m_Categories);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddCategory()
        {
            Category m_Category = new Category();

            return View("AddCategory", m_Category);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult AddCategory(Category m_Category)
        {
            if (ModelState.IsValid)
            {
                CategoryRepository.Create(m_Category);
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return View("AddCategory", m_Category);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            Category m_Category = CategoryRepository.RetrieveOne(id);
            return View("EditCategory", m_Category);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult EditCategory(Category m_Category)
        {
            if (ModelState.IsValid)
            {
                CategoryRepository.Update(m_Category);
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return View("EditCategory", m_Category);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            CategoryRepository.Delete(id);
            return RedirectToAction("Index", "Category");
        }
    }
}
