using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class EventRepository : IEventRepository
    {
        public bool Create(Event m_Event)
        {
            DBEvent.Create(m_Event);
            return true;
        }

        public Event RetrieveOne(int m_Eid)
        {
            Event m_Event = new Event();
            m_Event = DBEvent.RetrieveOne(m_Eid);

            return m_Event;
        }

        public List<Event> RetrieveAll(string myDate)
        {
            DateTime resultDate;
            List<Event> myEvents = new List<Event>();

            if (string.IsNullOrEmpty(myDate))
            {
                resultDate = DateTime.Today;
            }
            else
            {
                resultDate = DateTime.Parse(myDate);
            }

            myEvents = DBEvent.RetrieveAll(resultDate);

            return myEvents;
        }
        public bool Update(Event m_Event)
        {
            DBEvent.Update(m_Event);
            return true;
        }
        public bool Delete(int m_Eid)
        {
            DBEvent.Delete(m_Eid);
            return true;
        }

        public bool EventStartTimeErrorChecking(Event m_Event)
        {
            if (m_Event.EventStartHour != -1 && (m_Event.EventStartMin == -1 || m_Event.AmpmStart == "-1"))
            {
                return false;
            }
            if (m_Event.EventStartMin != -1 && (m_Event.EventStartHour == -1 || m_Event.AmpmStart == "-1"))
            {
                return false;
            }
            if (m_Event.AmpmStart != "-1" && (m_Event.EventStartHour == -1 || m_Event.EventStartMin == -1))
            {
                return false;
            }

            return true;
        }

        public bool EventEndTimeErrorChecking(Event m_Event)
        {
            if (m_Event.EventEndHour != -1 && (m_Event.EventEndMin == -1 || m_Event.AmpmEnd == "-1"))
            {
                return false;     
            }
            if (m_Event.EventEndMin != -1 && (m_Event.EventEndHour == -1 || m_Event.AmpmEnd == "-1"))
            {
                return false;
            }
            if (m_Event.AmpmEnd != "-1" && (m_Event.EventEndHour == -1 || m_Event.EventEndMin == -1))
            {
                return false;
            }

            return true;
        }

        public void LockEvent(int id)
        {
            DBEvent.LockEvent(id);
        }

        public void UnlockEvent(int id)
        {
            DBEvent.UnlockEvent(id);
        }

        public void PublishEvent(int id)
        {
            DBEvent.PublishEvent(id);
        }

        public List<Event> getFeaturedEvents()
        {
            List<Event> m_Events = new List<Event>();
            m_Events = DBEvent.getFeaturedEvents();

            return m_Events;
        }
    }
}