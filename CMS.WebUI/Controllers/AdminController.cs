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
    public class AdminController : Controller
    {
        IAdminRepository AdminRepository;
        ITrashRepository TrashRepository;

        public AdminController(IAdminRepository AdminRepo, ITrashRepository TrashRepo)
        {
            AdminRepository = AdminRepo;
            TrashRepository = TrashRepo;
        }

        public ViewResult Login(int id = 0)
        {
            
            ViewBag.id = id;
            return View("Login");
        }

        [CMSAuth]
        public void LogOut()
        {
            Session.Clear();
            Response.Redirect("/Admin/Login");
        }

        [HttpPost]
        public void Process(string userName, string passWord)
        {
            if (SessionHandler.authenticate(userName, passWord))
            {
                Response.Redirect("/Admin/Index");
                Response.End();
            }
            else
            {
                if (HttpContext.Session["uid"] != null)
                {
                    if (SessionHandler.is_user_locked((int)HttpContext.Session["uid"]))
                    {
                        Response.Redirect("/Admin/Login/2");
                    }
                    else
                    {
                        Response.Redirect("/Admin/Login/1");
                    }
                }
                else
                {
                    Response.Redirect("/Admin/Login/1");
                }
                

                Response.End();
            }
        }

        [CMSAuth]
        public ViewResult Index()
        {
            return View();
        }

        [CMSAuth]
        public ActionResult getAwaitingApproval(int id = 0)
        {
            List<Admin> m_Admin = AdminRepository.getAwaitingApproval(id);
            return View("getAwaitingApproval", m_Admin);
        }

        [CMSAuth]
        public ActionResult getTrash()
        {
            List<Trash> m_Trash = TrashRepository.RetrieveAll();

            return View("getTrash", m_Trash);
        }

        [CMSAuth]
        public ActionResult getLockedContent(int pageNum = 0, int objectId = 0)
        {
            List<Admin> m_Admin = AdminRepository.getLockedContent(pageNum);

            return View("getLockedContent", m_Admin);
        }

        [CMSAuth]
        public ActionResult TrashRestore(int id = 0)
        {
            AdminRepository.TrashRestore(id);

            List<Trash> m_Trash = TrashRepository.RetrieveAll();

            return View("getTrash", m_Trash);
           
        }

        [CMSAuth]
        public ActionResult UnlockContent(string faqid, int id = 0)
        {
            AdminRepository.UnlockContent(faqid, id);
            List<Admin> m_Admin = AdminRepository.getLockedContent(0);

            return View("getLockedContent", m_Admin);
        }

        [CMSAuth]
        public ActionResult Preview(string faqid, int id = 0)
        {
            string objectType = faqid;

            if (objectType == "Page")
            {
                return RedirectToAction("PagePreview", "Page", new { id = id });
            }
            else if (objectType == "Calendar")
            {
                return RedirectToAction("EventPreview", "Calendar", new { id = id });
            }

            return View();
        }

    }
}
