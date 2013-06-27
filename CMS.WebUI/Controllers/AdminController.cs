using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.WebUI.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult PagesCreate()
        {
            return View("PagesCreate");
        }

    }
}
