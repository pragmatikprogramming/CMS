using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;

namespace CMS.WebUI.Controllers
{
    public class NewsController : Controller
    {
        INewsRepository NewsRepository;

        public NewsController(INewsRepository NewsRepo)
        {
            NewsRepository = NewsRepo;
        }

        public ActionResult Index()
        {
            ViewBag.Count = 0;
            List<BlogPost> m_BlogPosts = NewsRepository.GetNews();
            return View("News", m_BlogPosts);
        }

        public ActionResult getNewsContainer()
        {
            WidgetContainer m_Container = NewsRepository.GetNewsContainer();
            return View("Container", m_Container);
        }

        public ActionResult Article(int id)
        {
            BlogPost m_BlogPost = NewsRepository.GetArticle(id);

            if (m_BlogPost.RedirectUrl.Length > 0)
            {
                return Redirect(m_BlogPost.RedirectUrl);
            }

            return View("Article", m_BlogPost);

        }

    }
}
