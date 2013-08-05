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
            object myObject = new object();
            return (Event)myObject;
        }

        public List<Event> RetrieveAll()
        {
            List<Event> myList = new List<Event>();
            return myList;
        }
        public bool Update(Event m_Event)
        {
            return true;
        }
        public bool Delete(int m_Eid)
        {
            return true;
        }
    }
}