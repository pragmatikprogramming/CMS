using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.DataAccess;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;

namespace CMS.Domain.Models
{
    public class HTMLWidgetRepository : IHTMLWidgetRepository
    {
        public void Create(HTMLWidget m_Widget)
        {
            DBHTMLWidget.Create(m_Widget);
        }
        public HTMLWidget RetrieveOne(int id)
        {
            HTMLWidget m_Widget = DBHTMLWidget.RetrieveOne(id);
            return m_Widget;
        }

        public List<HTMLWidget> RetrieveAll()
        {
            List <HTMLWidget> m_Widgets = DBHTMLWidget.RetrieveAll();
            return m_Widgets;
        }

        public List<HTMLWidget> RetrieveAll(int id)
        {
            List<HTMLWidget> m_Widgets = new List<HTMLWidget>();
            return m_Widgets;
        }

        public void Update(HTMLWidget m_Widget)
        {
            DBHTMLWidget.Update(m_Widget);
        }

        public void Delete(int id)
        {
            DBHTMLWidget.Delete(id);
        }
    }
}