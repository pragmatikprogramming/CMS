using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Entities;
using CMS.Domain.Abstract;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class FormFieldController : Controller
    {
        IFormFieldRepository FormFieldRepository;

        public FormFieldController(IFormFieldRepository FormFieldRepo)
        {
            FormFieldRepository = FormFieldRepo;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            List<FormField> m_FormFields = FormFieldRepository.RetrieveAll();
            return View("Index", m_FormFields);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddFormField()
        {
            FormField m_FormField = new FormField();
            ViewBag.FieldTypes = FormFieldRepository.getFieldTypes();
            ViewBag.ValidationTypes = FormFieldRepository.getValidationTypes();
            return View("AddFormField", m_FormField);
        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddFormField(FormField m_FormField, string[] childrenTitle, string[] childrenValue)
        {
            ViewBag.DisplayInfo = 0;

            if (childrenTitle == null)
            {
                ViewBag.childrenTitle = new string[0];
            }
            else
            {
                ViewBag.childrenTitle = childrenTitle;
            }

            if (childrenValue == null)
            {
                ViewBag.childrenTitle = new string[0];
            }
            else
            {
                ViewBag.childrenValue = childrenValue;
            }

            ViewBag.FieldTypes = FormFieldRepository.getFieldTypes();
            ViewBag.ValidationTypes = FormFieldRepository.getValidationTypes();

            if (ModelState.IsValid)
            {
                FormFieldRepository.Create(m_FormField, childrenTitle, childrenValue);
                return RedirectToAction("Index", "FormField");
            }
            else
            {
                ViewBag.DisplayInfo = 1;
                if (m_FormField.FieldType == 3)
                {
                    ViewBag.Type = "Checkbox";
                }
                else if (m_FormField.FieldType == 4)
                {
                    ViewBag.Type = "Radio";
                }
                else if (m_FormField.FieldType == 5)
                {
                    ViewBag.Type = "Option";
                }

                return View("AddFormField", m_FormField);
            }
            
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditFormField(int id)
        {
            FormField m_FormField = FormFieldRepository.RetrieveOne(id);
            ViewBag.FieldTypes = FormFieldRepository.getFieldTypes();
            ViewBag.ValidationTypes = FormFieldRepository.getValidationTypes();
            return View("EditFormField", m_FormField);
        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditFormField(FormField m_FormField, string[] childrenTitle, string[] childrenValue)
        {
            ViewBag.DisplayInfo = 0;

            if (ModelState.IsValid)
            {
                FormFieldRepository.Update(m_FormField, childrenTitle, childrenValue);
                return RedirectToAction("Index", "FormField");
            }
            else
            {
                ViewBag.DisplayInfo = 1;
                ViewBag.childrenTitle = childrenTitle;
                ViewBag.FieldTypes = FormFieldRepository.getFieldTypes();
                ViewBag.ValidationTypes = FormFieldRepository.getValidationTypes();

                if (m_FormField.FieldType == 3)
                {
                    ViewBag.Type = "Checkbox";
                }
                else if (m_FormField.FieldType == 4)
                {
                    ViewBag.Type = "Radio";
                }
                else if (m_FormField.FieldType == 5)
                {
                    ViewBag.Type = "Option";
                }

                return View("EditFormField", m_FormField);
            }
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult UpdateForm(int fieldType)
        {
            ViewBag.FieldType = fieldType;
            return View("UpdateForm");
        }

        [CMSAuth]
        public ActionResult AddOption(string id)
        {
            ViewBag.FieldType = id;
            return View("AddOption");
        }

        [CMSAuth]
        public ActionResult FormFieldDelete(int id)
        {
            FormFieldRepository.Delete(id);
            return RedirectToAction("Index", "FormField");
        }
    }
}
