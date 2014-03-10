using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;
using CMS.Domain.HelperClasses;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class DocumentController : Controller
    {
        private IFolderRepository FolderRepository;
        private IDocumentRepository DocumentRepository;

        public DocumentController(IFolderRepository FolderRepo, IDocumentRepository DocumentRepo)
        {
            FolderRepository = FolderRepo;
            DocumentRepository = DocumentRepo;
        }

        [CMSAuth]
        public ActionResult Index(int id = 0)
        {
            ViewBag.ParentId = id;
            List<Folder> m_Folders = FolderRepository.RetrieveAll(id);
            List<Document> m_Documents = DBDocument.RetriveAll(id);
            ViewBag.Documents = m_Documents;
            ViewBag.CurrentFolder = FolderRepository.RetrieveOne(id);
            return View("Document", m_Folders);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult MoveDoc(int parentId, int id)
        {
            ViewBag.ParentId = parentId;
            ViewBag.DocId = id;
            List<Folder> m_Folders = FolderRepository.RetrieveAll(parentId);
            return View("MoveDoc", m_Folders);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult MoveDocComplete(int parentId, int id)
        {
            DocumentRepository.MoveDoc(parentId, id);
            return RedirectToAction("Index", "Document", new { id = parentId });
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddFolder(int id = 0)
        {

            ViewBag.ParentId = id;

            Folder m_Folder = new Folder();

            return View("AddFolder", m_Folder);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult AddFolder(Folder m_Folder)
        {
            if (ModelState.IsValid)
            {
                FolderRepository.Create(m_Folder);
                return RedirectToAction("Index", "Document", new { id = m_Folder.ParentId });
            }
            else
            {
                ViewBag.ParentId = m_Folder.ParentId;
                return View("AddFolder", m_Folder);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditFolder(int id)
        {
            Folder m_Folder = FolderRepository.RetrieveOne(id);

            ViewBag.OldName = m_Folder.Name;

            return View("EditFolder", m_Folder);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult EditFolder(Folder m_Folder, string OldName)
        {
            ViewBag.OldName = OldName;

            if (ModelState.IsValid)
            {
                FolderRepository.Update(m_Folder, OldName);
                return RedirectToAction("Index", "Document", new { id = m_Folder.ParentId });
            }
            else
            {
                return View("EditFolder", m_Folder);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult DeleteFolder(int id)
        {
            Folder m_Folder = FolderRepository.RetrieveOne(id);

            if (!DBFolder.FolderCheckChildren(id))
            {
                ModelState.AddModelError("Name", "The folder you are trying to delete contains content that has not been permanently deleted. Please delete the content prior to deleting the folder.");
            }

            if (ModelState.IsValid)
            {
                FolderRepository.Delete(id);
                return RedirectToAction("Index", "Document", new { id = m_Folder.ParentId });
            }
            else
            {
                ViewBag.ParentId = m_Folder.ParentId;

                List<Folder> m_Folders = FolderRepository.RetrieveAll(m_Folder.ParentId);
                List<Document> m_Documents = DBDocument.RetriveAll(m_Folder.ParentId);
                ViewBag.Documents = m_Documents;
                return View("Document", m_Folders);
            }

        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddDocument(int id = 0)
        {
            ViewBag.ParentId = id;
            ViewBag.myContentGroups = Utility.ContentGroups();

            Document m_Document = new Document();
            return View("AddDocument", m_Document);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult AddDocument(Document m_Document, HttpPostedFileBase fileUpload)
        {
            ViewBag.ParentId = m_Document.ParentId;
            ViewBag.myContentGroups = Utility.ContentGroups();

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                if (!Utility.mimeTypeAllowed(fileUpload.ContentType))
                {
                    ModelState.AddModelError("Name", "This document type is not allowed");
                }
            }
            else
            {
                ModelState.AddModelError("Name", "Please chose a file to upload");
            }

            if (ModelState.IsValid)
            {
               DocumentRepository.Create(m_Document, fileUpload);
               return RedirectToAction("Index", "Document", new { id = m_Document.ParentId });
            }
            else
            {
                return View("AddDocument", m_Document);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditDocument(int id)
        {
            ViewBag.myContentGroups = Utility.ContentGroups();
            Document m_Document = DBDocument.RetrieveOne(id);

            return View("", m_Document);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult EditDocument(Document m_Document, HttpPostedFileBase fileUpload, string OldName)
        {
            ViewBag.myContentGroups = Utility.ContentGroups();

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                if (!Utility.mimeTypeAllowed(fileUpload.ContentType))
                {
                    ModelState.AddModelError("Name", "This document type is not allowed");
                }
            }

            if (ModelState.IsValid)
            {
                DocumentRepository.Update(m_Document, OldName, fileUpload);
                return RedirectToAction("Index", "Document", new { id = m_Document.ParentId });
            }
            else
            {
                return View("EditDocument", m_Document);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult DeleteDocument(int id)
        {
            Document m_Document = DocumentRepository.RetrieveOne(id);
            DocumentRepository.Delete(id);

            return RedirectToAction("Index", "Document", new { id = m_Document.ParentId });
        }
    }
}
