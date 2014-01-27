using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IEventRepository
    {
        bool Create(Event m_Event);
        Event RetrieveOne(int m_Eid);
        List<Event> RetrieveAll(string mDate);
        bool Update(Event m_Event);
        bool Delete(int m_Eid);
        bool EventStartTimeErrorChecking(Event m_Event);
        bool EventEndTimeErrorChecking(Event m_Event);
        void LockEvent(int id);
        void UnlockEvent(int id);
        void PublishEvent(int id);
        List<Event> getFeaturedEvents();
        bool EventTimeBothErrorChecking(Event m_Event);
        bool EventStartTimeBeforeEventEndTime(Event m_Event);
    }
}
