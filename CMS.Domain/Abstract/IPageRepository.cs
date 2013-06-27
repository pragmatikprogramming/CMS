using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IPageRepository
    {
        void Create(Page m_Page);
        Page Retrieve(int m_ID);
        bool Update(Page m_Page);
        void Delete(int m_ID);
    }
}
