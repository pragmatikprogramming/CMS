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
    public class BlogPostController : Controller
    {
        IBlogPostRepository BlogPostRepository;
        IImageRepository ImageRepository;

        public BlogPostController(IBlogPostRepository BlogPostRepo, IImageRepository ImageRepo)
        {
            BlogPostRepository = BlogPostRepo;
            ImageRepository = ImageRepo;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            ViewBag.Categories = BlogPostRepository.getCategories();
            List<BlogPost> m_BlogPosts = BlogPostRepository.RetrieveAll();
            ViewBag.m_SessionId = (int)System.Web.HttpContext.Current.Session["uid"];
            return View("Index", m_BlogPosts);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddBlogPost()
        {
            ViewBag.Categories = BlogPostRepository.getCategories();
            ViewBag.myContentGroups = Utility.ContentGroups();
            BlogPost m_BlogPost = new BlogPost();
            ViewBag.NewsImages = ImageRepository.RetrieveAll(23);

            return View("AddBlogPost", m_BlogPost);
        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddBlogPost(BlogPost m_BlogPost)
        {

            if (ModelState.IsValid)
            {
                BlogPostRepository.Create(m_BlogPost);
                return RedirectToAction("Index", "BlogPost");
            }
            else
            {
                ViewBag.Categories = BlogPostRepository.getCategories();
                ViewBag.myContentGroups = Utility.ContentGroups();
                ViewBag.NewsImages = ImageRepository.RetrieveAll(23);
                return View("AddBlogPost", m_BlogPost);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditBlogPost(int id)
        {
            ViewBag.Categories = BlogPostRepository.getCategories();
            ViewBag.myContentGroups = Utility.ContentGroups();
            BlogPost m_BlogPost = BlogPostRepository.RetrieveOne(id);
            ViewBag.NewsImages = ImageRepository.RetrieveAll(23);
            return View("EditBlogPost", m_BlogPost);
        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditBlogPost(BlogPost m_BlogPost)
        {
            BlogPost a_BlogPost = BlogPostRepository.RetrieveOne(m_BlogPost.Id);
            m_BlogPost.PageWorkFlowState = a_BlogPost.PageWorkFlowState;
            if (ModelState.IsValid)
            {
                m_BlogPost.BlogPostSet();
                BlogPostRepository.Update(m_BlogPost);
                return RedirectToAction("Index", "BlogPost");
            }
            else
            {
                if (m_BlogPost.Categories == null)
                {
                    List<int> mCats = new List<int>();
                    m_BlogPost.Categories = mCats;
                }
                ViewBag.Categories = BlogPostRepository.getCategories();
                ViewBag.myContentGroups = Utility.ContentGroups();
                ViewBag.NewsImages = ImageRepository.RetrieveAll(23);
                return View("EditBlogPost", m_BlogPost);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult BlogDelete(int id)
        {
            //id is actually blogId

            BlogPostRepository.Delete(id);
            return RedirectToAction("Index", "BlogPost");
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult BlogUnlock(int id)
        {
            //id is actually blogId

            BlogPostRepository.unlockBlogPost(id);
            return RedirectToAction("Index", "BlogPost");
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult BlogPublish(int id)
        {
            BlogPost m_BlogPost = BlogPostRepository.RetrieveOne(id);
            BlogPostRepository.publishBlogPost(id);
            BlogPostRepository.unlockBlogPost(m_BlogPost.BlogId);
            return RedirectToAction("Index", "BlogPost");
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult BlogPreview(int id)
        {
            BlogPost m_BlogPost = BlogPostRepository.RetrieveOne(id);
            ViewBag.PageType = 5;
            ViewBag.PageId = null;

            return View("HomeInterior", m_BlogPost); 
        }

        [CMSAuth]
        public ActionResult getCategoryFilter(int Categories)
        {
            List<BlogPost> m_BlogPosts = BlogPostRepository.RetrieveAllByCategory(Categories);
            ViewBag.m_SessionId = (int)System.Web.HttpContext.Current.Session["uid"];

            return PartialView("getCategoryFilter", m_BlogPosts);
        }

        [CMSAuth]
        public ActionResult ImageSelect(string ImageName, int ImageId)
        {
            ViewBag.ImageName = ImageName;
            ViewBag.ImageId = ImageId;
            return View("ImageSelect");
        }

        [CMSAuth]
        public ActionResult BlogPostComments(int id)
        {
            List<BlogPostComment> m_Comments = BlogPostRepository.GetComments(id);
            return View("BlogPostComments", m_Comments);

        }

        [CMSAuth]
        public ActionResult CommentPublish(int parentId, int id)
        {
            BlogPostRepository.CommentPublish(id);
            return RedirectToAction("BlogPostComments", new { id = parentId });
        }

        [CMSAuth]
        public ActionResult CommentDelete(int parentId, int id)
        {
            BlogPostRepository.CommentDelete(id);
            return RedirectToAction("BlogPostComments", new { id = parentId });
        }
    }
}
