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
            ViewBag.Branchs = Utility.BranchNames();
            ViewBag.myContentGroups = Utility.ContentGroups();

            if (!EventRepository.EventStartTimeErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventStartHour", "A complete start time including AM or PM is required");
            }

            if (!EventRepository.EventEndTimeErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventEndHour", "A complete end time including AM or PM is required");
            }

            if (!EventRepository.EventTimeBothErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventStartHour", "You cannot input an end time without a start time");
            }

            if (!EventRepository.EventStartTimeBeforeEventEndTime(m_Event))
            {
                ModelState.AddModelError("EventStartHour", "Start Time must be before End Time");
            }

            if (ModelState.IsValid)
            {
                EventRepository.Create(m_Event);

                return RedirectToAction("Index", "Calendar", new { id = DateTime.Parse(m_Event.EventStartDate.ToString()).ToString("MM-dd-yyyy")});
            }
            else
            {
                return View("Add", m_Event);
            }
        }

        [CMSAuth]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Event m_Event = EventRepository.RetrieveOne(id);

            ViewBag.myContentGroups = Utility.ContentGroups();
            ViewBag.Branchs = Utility.BranchNames();

            if (m_Event.LockedBy > 0 && m_Event.LockedBy != (int)System.Web.HttpContext.Current.Session["uid"])
            {
                return RedirectToAction("Index", "Calendar");
            }
            else
            {
                EventRepository.LockEvent(id);
                return View("Edit", m_Event);
            }
        }
        
        [CMSAuth]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Event m_Event)
        {
            ViewBag.myContentGroups = Utility.ContentGroups();
            ViewBag.Branchs = Utility.BranchNames();

            if (!EventRepository.EventStartTimeErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventStartHour", "A complete start time including AM or PM is required");
            }

            if (!EventRepository.EventEndTimeErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventEndHour", "A complete end time including AM or PM is required");
            }

            if (!EventRepository.EventTimeBothErrorChecking(m_Event))
            {
                ModelState.AddModelError("EventStartHour", "You cannot input an end time without a start time");
            }

            if (!EventRepository.EventStartTimeBeforeEventEndTime(m_Event))
            {
                ModelState.AddModelError("EventStartHour", "Start Time must be before End Time");
            }

            if (m_Event.LockedBy > 0 && m_Event.LockedBy != (int)System.Web.HttpContext.Current.Session["uid"])
            {
                ModelState.AddModelError("EventTitle", "This Event is currently locked and not editable");
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
        public ActionResult Delete(int id)
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

        [CMSAuth]
        public ActionResult EventPublish(int id)
        {
            EventRepository.PublishEvent(id);
            return RedirectToAction("Index", "Calendar");
        }

        [CMSAuth]
        public ActionResult EventUnlock(int id)
        {
            EventRepository.UnlockEvent(id);
            return RedirectToAction("Index", "Calendar");
        }

        [CMSAuth]
        public ActionResult EventPreview(int id = 0)
        {
            Event m_Event = EventRepository.RetrieveOne(id);
            ViewBag.Content = m_Event.Body;

            return View("CalendarPreview", m_Event);
        }

        [CMSAuth]
        public ActionResult getContent(int id = 0)
        {
            Event m_Event = EventRepository.RetrieveOne(id);

            return View("getContent", m_Event);
        }
    }
}
