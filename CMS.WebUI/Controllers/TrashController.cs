using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Models;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class TrashController : Controller
    {
        ITrashRepository TrashRepository;

        public TrashController(ITrashRepository TrashRepo)
        {
            TrashRepository = TrashRepo;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            List<Trash> m_Trash = TrashRepository.RetrieveAll();

            return View("Trash", m_Trash);
        }

        [CMSAuth]
        public ActionResult Restore(int id)
        {
            TrashRepository.Restore(id);

            return RedirectToAction("Index", "Trash");
        }

        [CMSAuth]
        public ActionResult Delete(int id)
        {
            TrashRepository.Delete(id);

            return RedirectToAction("Index", "Trash");
        }
    }
}
