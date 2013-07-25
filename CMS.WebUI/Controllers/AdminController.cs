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
        //
        // GET: /Admin/

        public ViewResult Login()
        {
            int id = 0;
            int.TryParse((string)Url.RequestContext.RouteData.Values["id"], out id);
            ViewBag.id = id;
            return View("Login");
        }

        [HttpPost]
        public void Process(string userName, string passWord)
        {
            if (SessionHandler.authenticate(userName, passWord))
            {
                Response.Redirect("/Admin/");
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

        public ViewResult PagesCreate()
        {
            return View("PagesCreate");
        }

        public ViewResult MediaManager()
        {
            return View("MediaManager");
        }
    }
}
