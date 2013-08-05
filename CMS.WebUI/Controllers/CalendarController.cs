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

        public ActionResult Index()
        {
            List<List<int>> myCal = new List<List<int>>();
            myCal = CMSCalendar.loadDays(DateTime.Today);
            ViewBag.Today = DateTime.Today.Day;
        
            return View("ViewFull", myCal);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Event m_Event = new Event();
            return View("Add", m_Event);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Event m_Event)
        {
            //Response.Write(DateTime.Parse(m_Event.EventEndDate.ToString("MM/dd/yyyy") + " " + m_Event.EventEndHour.ToString() + ":" + m_Event.EventEndMin.ToString() + " " + m_Event.AmpmEnd));
            //Response.End();
            if (ModelState.IsValid)
            {
                DBEvent.Create(m_Event);
                List<List<int>> myCal = new List<List<int>>();
                myCal = CMSCalendar.loadDays(DateTime.Today);
                ViewBag.Today = DateTime.Today.Day;

                return View("ViewFull", myCal);
            }
            else
            {
                return View("Add", m_Event);
            }
        }
    }
}
