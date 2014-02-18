using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.HelperClasses;
using CMS.Domain.Entities;
using CMS.WebUI.Infrastructure;


namespace CMS.WebUI.Controllers
{
    public class GalleryController : Controller
    {
        private IGalleryRepository GalleryRepository;
        private IImageRepository ImageRepository;

        public GalleryController(IGalleryRepository m_GalleryRepository, IImageRepository m_ImageRepository)
        {
            GalleryRepository = m_GalleryRepository;
            ImageRepository = m_ImageRepository;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            List<Gallery> myFolders = GalleryRepository.RetrieveAll();
            return View("Gallery", myFolders);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddGallery()
        {
            ViewBag.myContentGroups = Utility.ContentGroups();
            Gallery m_Gallery = new Gallery();
            return View("AddGallery", m_Gallery);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult AddGallery(Gallery m_Gallery)
        {
            ViewBag.myContentGroups = Utility.ContentGroups();

            if (ModelState.IsValid)
            {
                GalleryRepository.CreateGallery(m_Gallery);
                return RedirectToAction("Index", "Gallery");
            }
            else
            {
                return View("AddGallery", m_Gallery);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditGallery(string id)
        {
            int m_ID = int.Parse(id);
            ViewBag.myContentGroups = Utility.ContentGroups();

            Gallery m_Gallery = GalleryRepository.RetrieveOne(m_ID);

            return View("EditGallery", m_Gallery);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult EditGallery(Gallery m_Gallery, string OldName)
        {
            ViewBag.myContentGroups = Utility.ContentGroups();

            if (ModelState.IsValid)
            {
                GalleryRepository.Update(m_Gallery, OldName);
                return RedirectToAction("Index", "Gallery");
            }
            else
            {
                return View("EditGallery", m_Gallery);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult DeleteGallery(string id)
        {
            int Id = int.Parse(id);

            if (!GalleryRepository.DeleteGallery(Id))
            {
                ModelState.AddModelError("Name", "Unable to delete Gallery.  There are images currently associated with this Gallery");
            }

            List<Gallery> myFolders = GalleryRepository.RetrieveAll();
            return View("Gallery", myFolders);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult Images(int id)
        {
            
            List<Image> m_Images = ImageRepository.RetrieveAll(id);
            Gallery m_Gallery = GalleryRepository.RetrieveOne(id);
            ViewBag.Id = id;
            ViewBag.GalleryName = m_Gallery.Name;

            return View("Images", m_Images);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddImage(string id)
        {
            int m_Id = int.Parse(id);

            Image m_Image = new Image();
            HttpPostedFileBase file = null;
            ViewBag.file = file;
            ViewBag.myContentGroups = Utility.ContentGroups();
            ViewBag.Id = m_Id;

            return View("AddImage", m_Image);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult AddImage(Image m_Image, HttpPostedFileBase fileUpload)
        {
            ViewBag.file = fileUpload;
            ViewBag.myContentGroups = Utility.ContentGroups();
            ViewBag.Id = m_Image.ParentId;

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                if (fileUpload.ContentType != "image/jpg" && fileUpload.ContentType != "image/jpeg" && fileUpload.ContentType != "image/png" && fileUpload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Name", "An invalid file type was uploaded.  Please select a different file");
                }
            }
            else
            {
                ModelState.AddModelError("Name", "Please chose a file to upload");
            }

            if (ModelState.IsValid)
            {
                ImageRepository.Create(m_Image, fileUpload);
                return RedirectToAction("Images", "Gallery", new { id = m_Image.ParentId });
            }
            else
            {
                return View("AddImage", m_Image);
            }
        }
        
        [CMSAuth]
        [HttpGet]
        public ActionResult EditImage(string id)
        {
            int m_Id = int.Parse(id);

            Image m_Image = ImageRepository.RetrieveOne(m_Id);
            ViewBag.myContentGroups = Utility.ContentGroups();

            return View("EditImage", m_Image);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult EditImage(Image m_Image, HttpPostedFileBase fileUpload, string OldName, string OldFileType)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                if (fileUpload.ContentType != "image/jpg" && fileUpload.ContentType != "image/jpeg" && fileUpload.ContentType != "image/png" && fileUpload.ContentType != "image/gif" && fileUpload.ContentType != "image/bmp")
                {
                    ModelState.AddModelError("Name", "An invalid file type was uploaded.  Please select a different file");
                }
            }

            if (ModelState.IsValid)
            {
                ImageRepository.Update(m_Image, fileUpload, OldName, OldFileType);
                return RedirectToAction("Images", "Gallery", new { id = m_Image.ParentId });
            }
            else
            {
                ViewBag.myContentGroups = Utility.ContentGroups();
                return View("EditImage", m_Image);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult DeleteImage(int id)
        {
            Image m_Image = ImageRepository.RetrieveOne(id);
            ImageRepository.Delete(id);

            return RedirectToAction("Images", "Gallery", new { id = m_Image.ParentId });
        }
    }
}
