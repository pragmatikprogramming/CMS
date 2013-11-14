using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IHTMLWidgetRepository
    {
        void Create(HTMLWidget m_Widget);
        HTMLWidget RetrieveOne(int id);
        List<HTMLWidget> RetrieveAll();
        List<HTMLWidget> RetrieveAll(int id);
        void Update(HTMLWidget w_Widget);
        void Delete(int id);
    }
}
