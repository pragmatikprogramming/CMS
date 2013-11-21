using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IWidgetContainer
    {
        int Create(WidgetContainer m_Container);
        WidgetContainer RetrieveOne(int id);
        List<WidgetContainer> RetrieveAll();
        List<HTMLWidget> RetrieveAll(int id);
        void Update(WidgetContainer m_Container);
        void Delete(int id);
        List<HTMLWidget> getWidgets();
        void SortUp(int containerId, int widgetId);
        void SortDown(int containerId, int widgetId);
    }
}
