using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;

namespace CMS.WebUI.Controllers
{
    public class WidgetNewsRotatorController : Controller
    {
        IBlogPostRepository BlogPostRepository;

        public WidgetNewsRotatorController()
        {

        }

        public WidgetNewsRotatorController(IBlogPostRepository BlogPostRepo)
        {
            BlogPostRepository = BlogPostRepo;
        }

        public ActionResult Index()
        {
            List<BlogPost> m_BlogPosts = BlogPostRepository.RetrieveAllByCategory(2);
            List<int> m_Ids = BlogPostRepository.getNewsRotatorBlogIds();

            ViewBag.SortOrder1 = m_Ids[0];
            ViewBag.SortOrder2 = m_Ids[1];
            ViewBag.SortOrder3 = m_Ids[2];
            ViewBag.SortOrder4 = m_Ids[3];
            ViewBag.SortOrder5 = m_Ids[4];
            ViewBag.SortOrder6 = m_Ids[5];

            return View(m_BlogPosts);
        }

        public ActionResult SetOrder(int sortOrder1, int sortOrder2, int sortOrder3, int sortOrder4, int sortOrder5, int sortOrder6)
        {
            string msg = "";
            if (sortOrder1 == 0 || sortOrder2 == 0 || sortOrder3 == 0 || sortOrder4 == 0 || sortOrder5 == 0 || sortOrder6 == 0)
            {
                msg = "You must select a news article for each spot";
            }
            else
            {
                msg = "Items saved to widget";
            }

            ViewBag.Message = msg;
            ViewBag.SortOrder1 = sortOrder1;
            ViewBag.SortOrder2 = sortOrder2;
            ViewBag.SortOrder3 = sortOrder3;
            ViewBag.SortOrder4 = sortOrder4;
            ViewBag.SortOrder5 = sortOrder5;
            ViewBag.SortOrder6 = sortOrder6;

            if (msg == "Items saved to widget")
            {
                List<int> m_NewsRotator = new List<int>();
                m_NewsRotator.Add(sortOrder1);
                m_NewsRotator.Add(sortOrder2);
                m_NewsRotator.Add(sortOrder3);
                m_NewsRotator.Add(sortOrder4);
                m_NewsRotator.Add(sortOrder5);
                m_NewsRotator.Add(sortOrder6);

                BlogPostRepository.newsSortOrder(m_NewsRotator);

                List<BlogPost> m_BlogPosts = BlogPostRepository.RetrieveAllByCategory(2);
                return View("Index", m_BlogPosts);
            }
            else
            {
                List<BlogPost> m_BlogPosts = BlogPostRepository.RetrieveAllByCategory(2);
                return View("Index", m_BlogPosts);
            }
        }

    }
}
