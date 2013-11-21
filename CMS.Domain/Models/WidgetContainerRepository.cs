using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.DataAccess;
using CMS.Domain.Entities;

namespace CMS.Domain.Models
{
    public class WidgetContainerRepository : IWidgetContainer
    {
        public int Create(WidgetContainer m_Container)
        {
            int containerId = DBWidgetContainer.Create(m_Container);
            return containerId;
        }

        public WidgetContainer RetrieveOne(int id)
        {
            WidgetContainer m_Container = DBWidgetContainer.RetrieveOne(id);
            return m_Container;
        }

        public List<WidgetContainer> RetrieveAll()
        {
            List<WidgetContainer> m_Containers = DBWidgetContainer.RetrieveAll();
            return m_Containers;
        }

        public List<HTMLWidget> RetrieveAll(int id)
        {
            List<HTMLWidget> m_Widgets = DBWidgetContainer.RetrieveAll(id);
            return m_Widgets;
        }

        public void Update(WidgetContainer m_Container)
        {
            DBWidgetContainer.Update(m_Container);
        }

        public void Delete(int id)
        {
            DBWidgetContainer.Delete(id);
        }

        public List<HTMLWidget> getWidgets()
        {
            List<HTMLWidget> m_Widgets = DBWidgetContainer.getWidgets();
            return m_Widgets;
        }

        public void SortUp(int containerId, int widgetId)
        {
            DBWidgetContainer.SortUp(containerId, widgetId);
        }

        public void SortDown(int containerId, int widgetId)
        {
            DBWidgetContainer.SortDown(containerId, widgetId);
        }
    }
}