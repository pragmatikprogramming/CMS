using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Models;
using CMS.Domain.Entities;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class FormController : Controller
    {
        IFormRepository FormRepository;
        IFormFieldRepository FormFieldRepository;

        public FormController(IFormRepository FormRepo, IFormFieldRepository FormFieldRepo)
        {
            FormRepository = FormRepo;
            FormFieldRepository = FormFieldRepo;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            List<Form> m_Forms = FormRepository.RetrieveAll();
            return View("Index", m_Forms);
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult AddForm()
        {
            Form m_Form = new Form();
            ViewBag.FormFields = FormFieldRepository.RetrieveAll();
            return View("AddForm", m_Form);
        }

        [HttpPost]
        [CMSAuth]
        [ValidateInput(false)]
        public ActionResult AddForm(Form m_Form)
        {
            if (ModelState.IsValid)
            {
                int m_FormId = FormRepository.Create(m_Form);
                return RedirectToAction("OrderFormFields", "Form", new { id = m_FormId });
            }
            else
            {
                ViewBag.FormFields = FormFieldRepository.RetrieveAll();
                return View("AddForm", m_Form);
            }
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult EditForm(int id)
        {
            Form m_Form = FormRepository.RetrieveOne(id);
            ViewBag.FormFields = FormFieldRepository.RetrieveAll();
            return View("EditForm", m_Form);
        }

        [HttpPost]
        [CMSAuth]
        [ValidateInput(false)]
        public ActionResult EditForm(Form m_Form)
        {
            if (ModelState.IsValid)
            {
                FormRepository.Update(m_Form);
                return RedirectToAction("Index", "Form");
            }
            else
            {
                m_Form.MyFormFields = new List<int>();
                ViewBag.FormFields = FormFieldRepository.RetrieveAll();
                return View("EditForm", m_Form);
            }
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult FormDelete(int id)
        {
            FormRepository.Delete(id);
            return RedirectToAction("Index", "Form");
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult OrderFormFields(int id)
        {
            ViewBag.FormId = id;
            List<FormField> m_FormFields = FormRepository.getFormFields(id);
            return View("OrderFormFields", m_FormFields);
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult SortUp(int parentId, int id)
        {
            FormRepository.SortUp(parentId, id);
            return RedirectToAction("OrderFormFields", "Form", new { id = parentId });
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult SortDown(int parentId, int id)
        {
            FormRepository.SortDown(parentId, id);
            return RedirectToAction("OrderFormFields", "Form", new { id = parentId });
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult Preview(int id)
        {
            ViewBag.PageType = "Form";
            ViewBag.id = id;
            return View("FormPreview");
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult getForm(int id)
        {
            Form m_Form = FormRepository.RetrieveOne(id);
            return View("getForm", m_Form);
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult ToggleRequired(int parentId, int id, int value)
        {
            FormRepository.ToggleRequired(parentId, id, value);
            return RedirectToAction("OrderFormFields", "Form", new { id = parentId });
        }

        [HttpGet]
        [CMSAuth]
        public ActionResult ExtractFormData()
        {
            List<Form> m_Forms = FormRepository.RetrieveAll();
            return View("Extract", m_Forms);
        }

        [HttpPost]
        [CMSAuth]
        public FileContentResult ExtractFormData(int FormId, string StartDate, string EndDate)
        {
            string csv = "";
            csv = FormRepository.FormDataExtract(FormId, StartDate, EndDate);
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Report123.csv");
        }
    }
}
