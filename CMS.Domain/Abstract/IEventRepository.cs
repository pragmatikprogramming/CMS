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
        List<Event> RetrieveAll();
        bool Update(Event m_Event);
        bool Delete(int m_Eid);
    }
}
