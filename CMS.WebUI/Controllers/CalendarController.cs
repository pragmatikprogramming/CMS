using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using CMS.WebUI.Infrastructure;
using CMS.Domain.DataAccess;


namespace CMS.WebUI.Controllers
{
    public class CalendarController : Controller
    {
        private IEventRepository EventRepository;

        public CalendarController(IEventRepository eventRepo)
        {
            EventRepository = eventRepo;
        }

        [CMSAuth]
        public ActionResult Index(string id = null)
        {
            string myDate = id;
            if (string.IsNullOrEmpty(myDate))
            {
                ViewBag.Today = DateTime.Today.Day;
                ViewBag.MyCal = CMSCalendar.loadDays(DateTime.Today);
                ViewBag.MyMonth = DateTime.Now.ToString("MMMM yyyy");
                ViewBag.MyDate = DateTime.Today; //.ToString("MM-dd-yyyy");
                ViewBag.NextMonth = DateTime.Today.AddMonths(1).ToString("MM-dd-yyyy");
                ViewBag.PreviousMonth = DateTime.Today.AddMonths(-1).ToString("MM-dd-yyyy");;
            }
            else
            {
                ViewBag.Today = DateTime.Parse(myDate).Day;
                ViewBag.MyCal = CMSCalendar.loadDays(DateTime.Parse(myDate));
                ViewBag.MyMonth = DateTime.Parse(myDate).ToString("MMMM yyyy");
                ViewBag.MyDate = DateTime.Parse(myDate);
                ViewBag.NextMonth = DateTime.Parse(myDate).AddMonths(1).ToString("MM-dd-yyyy");
                ViewBag.PreviousMonth = DateTime.Parse(myDate).AddMonths(-1).ToString("MM-dd-yyyy");
            }
        
            return View("ViewFull");
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Branchs = DBEvent.BranchNames();
            ViewBag.myContentGroups = DBEvent.ContentGroups();

            Event m_Event = new Event();
            return View("Add", m_Event);
        }

        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Event m_Event)
        {
            ViewBag.Branchs = DBEvent.BranchNames();
            ViewBag.myContentGroups = DBEvent.ContentGroups();

            if (!EventRepository.EventStartTimeErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventStartHour", "A complete start time including AM or PM is required");
            }

            if (!EventRepository.EventEndTimeErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventEndHour", "A complete end time including AM or PM is required");
            }
            
            if (ModelState.IsValid)
            {
                EventRepository.Create(m_Event);

                return RedirectToAction("Index", "Calendar");
            }
            else
            {
                return View("Add", m_Event);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Event m_Event = EventRepository.RetrieveOne(id);

            ViewBag.myContentGroups = DBEvent.ContentGroups();
            ViewBag.Branchs = DBEvent.BranchNames();

            return View("Edit", m_Event);
        }
        
        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Event m_Event)
        {
            ViewBag.myContentGroups = DBEvent.ContentGroups();
            ViewBag.Branchs = DBEvent.BranchNames();

            if (!EventRepository.EventStartTimeErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventStartHour", "A complete start time including AM or PM is required");
            }

            if (!EventRepository.EventEndTimeErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventEndHour", "A complete end time including AM or PM is required");
            }

            if (ModelState.IsValid)
            {
                EventRepository.Update(m_Event);
                return RedirectToAction("Index", "Calendar");
            }
            else
            {
                return View("Edit", m_Event);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            EventRepository.Delete(id);

            return RedirectToAction("Index", "Calendar");  
        }

        [CMSAuth]
        public ActionResult getCalendarData(string myDate = null)
        {
            List<Event> myEvents = new List<Event>();
            myEvents = EventRepository.RetrieveAll(myDate);

            return PartialView("CalendarData", myEvents);
        }
    }
}
