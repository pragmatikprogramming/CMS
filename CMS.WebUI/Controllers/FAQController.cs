using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;
using CMS.WebUI.Infrastructure;

namespace CMS.WebUI.Controllers
{
    public class FAQController : Controller
    {
        private IFAQRepository FAQRepository;

        public FAQController(IFAQRepository m_FAQRepository)
        {
            FAQRepository = m_FAQRepository;
        }

        [CMSAuth]
        public ActionResult Index()
        {
            List<FAQ> m_FAQs = FAQRepository.RetrieveAllFAQ();
            return View("Index", m_FAQs);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult AddFAQ()
        {
            ViewBag.myContentGroups = DBEvent.ContentGroups();
            FAQ m_FAQ = new FAQ();

            return View("AddFAQ", m_FAQ);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult AddFAQ(FAQ m_FAQ)
        {
            ViewBag.myContentGroups = DBEvent.ContentGroups();

            if (ModelState.IsValid)
            {
                FAQRepository.CreateFAQ(m_FAQ);
                return RedirectToAction("Index", "FAQ");
            }
            else
            {
                return View("AddFAQ", m_FAQ);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult DeleteFAQ(string id)
        {
            int myID = int.Parse(id);
            FAQRepository.DeleteFAQ(myID);

            return RedirectToAction("Index", "FAQ");
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult EditFAQ(string id)
        {
            ViewBag.myContentGroups = DBEvent.ContentGroups();

            int myID = int.Parse(id);
            FAQ m_FAQ = FAQRepository.RetrieveOneFAQ(myID);

            return View("EditFAQ", m_FAQ);
        }

        [CMSAuth]
        [HttpPost]
        public ActionResult EditFAQ(FAQ m_FAQ)
        {
            ViewBag.myContentGroups = DBEvent.ContentGroups();

            if (ModelState.IsValid)
            {
                FAQRepository.UpdateFAQ(m_FAQ);
                return RedirectToAction("Index", "FAQ");
            }
            else
            {
                return View("EditFAQ", m_FAQ);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult FAQQuestions(string id)
        {
            int myID = int.Parse(id);
            ViewBag.FAQID = myID;
            List<FAQQuestions> myFAQs = FAQRepository.RetrieveAllFAQQuestions(myID);
            return View("FAQQuestions", myFAQs);
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult FAQQuestionAdd(string id)
        {
            int FaqID = int.Parse(id);
            ViewBag.FAQID = FaqID;
            FAQQuestions myQuestion = new FAQQuestions();

            return View("FAQQuestionAdd", myQuestion);
        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FAQQuestionAdd(FAQQuestions myQuestion)
        {

            if (ModelState.IsValid)
            {
                FAQRepository.CreateFAQQuestion(myQuestion);
                return RedirectToAction("FAQQuestions", "FAQ", new { id = myQuestion.FaqID });
            }
            else
            {
                ViewBag.FAQID = myQuestion.FaqID;
                return View("FAQQuestionAdd", myQuestion);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult FAQQuestionEdit(string id)
        {
            int QID = int.Parse(id);

            FAQQuestions myQuestion = FAQRepository.RetrieveOneFAQQuestion(QID);

            return View("FAQQuestionEdit", myQuestion);

        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FAQQuestionEdit(FAQQuestions m_FAQQuestion)
        {
            if (ModelState.IsValid)
            {
                FAQRepository.UpdateFAQQuestion(m_FAQQuestion);
                return RedirectToAction("FAQQuestions", "FAQ", new { id = m_FAQQuestion.FaqID });
            }
            else
            {
                return View("FAQQuestionEdit", m_FAQQuestion);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult FAQQuestionDelete(string faqid, string id)
        {
            int QID = int.Parse(id);
            int FAQID = int.Parse(faqid);

            FAQRepository.DeleteFAQQuestion(QID);

            return RedirectToAction("FAQQuestions", "FAQ", new { id = FAQID });
            
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult sortUp(int id)
        {
            FAQQuestions m_Question = DBFAQ.RetrieveOneFAQQuestion(id);

            FAQRepository.sortUp(id);

            return RedirectToAction("FAQQuestions", "FAQ", new {id = m_Question.FaqID});
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult sortDown(int id)
        {
            FAQQuestions m_Question = DBFAQ.RetrieveOneFAQQuestion(id);

            FAQRepository.sortDown(id);

            return RedirectToAction("FAQQuestions", "FAQ", new { id = m_Question.FaqID });
        }
    }
}
