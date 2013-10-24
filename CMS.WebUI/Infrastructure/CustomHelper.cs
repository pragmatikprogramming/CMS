using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;
using CMS.Domain.DataAccess;
using CMS.Domain.Entities;

namespace CMS.WebUI.Infrastructure
{
    public static class CustomHelper
    {
        public static MvcHtmlString BreadCrumb(this HtmlHelper html, int parentId)
        {
            Page m_Page = DBPage.RetrieveOne(parentId);
            string m_BreadCrumb = "";

            if (parentId == 0)
            {
                m_BreadCrumb = "Root";
            }
            else if (m_Page.ParentId == 0)
            {
                m_BreadCrumb = "<a href='/Page/Index/0'>Root</a> > " + m_Page.NavigationName;
            }
            else
            {
                m_BreadCrumb = "<a href='/Page/Index/" + m_Page.ParentId.ToString() +"'>" + getBreadCrumb(m_Page.ParentId) + "</a>" + " > " + m_Page.NavigationName;
                m_BreadCrumb = "<a href='/Page/Index/0'>Root > </a>" + m_BreadCrumb;
            }

            return new MvcHtmlString(m_BreadCrumb.ToString());
        }

        private static string getBreadCrumb(int parentId)
        {
            Page m_Page = DBPage.RetrieveOne(parentId);
            string m_BreadCrumb = "";

            if (m_Page.ParentId == 0)
            {
                return m_Page.NavigationName;
            }
            else
            {
                m_BreadCrumb = "<a href='/Page/Index/" + parentId.ToString() + "'>" + getBreadCrumb(m_Page.ParentId) + "</a>";
                m_BreadCrumb += " > <a href='/Page/Index/" + parentId.ToString() + "'>" + m_Page.NavigationName + "</a>";
            }

            return m_BreadCrumb;
        }

        public static MvcHtmlString Menu(this HtmlHelper html)
        {
            List<Page> m_Pages = new List<Page>();
            m_Pages = DBPage.RetrieveAll(0);

            string m_Menu = "<ul>";

            foreach (Page m_Page in m_Pages)
            {
                int numChildren = DBPage.getNumChildren(m_Page.PageID);

                if (numChildren > 0)
                {
                    m_Menu += "<li><a href='#" + m_Page.PageID + "'  data-toggle='collapse'><i class='icon-plus'></i></a>&nbsp;&nbsp;<a data-ajax='true' data-ajax-mode='replace' data-ajax-update='#linkurl' href='/Menu/getLinkUrl/" + m_Page.PageID + "'>" + m_Page.NavigationName + "</a></li>";
                    m_Menu += getMenu(m_Page.PageID);
                }
                else
                {
                    m_Menu += "<li style='padding-left: 19px;'><a data-ajax='true' data-ajax-mode='replace' data-ajax-update='#linkurl' href='/Menu/getLinkUrl/" + m_Page.PageID + "'>" + m_Page.NavigationName + "</a></li>";
                }
            }

            m_Menu += "</ul>";
            return new MvcHtmlString(m_Menu.ToString());
        }

        private static string getMenu(int Id)
        {
            List<Page> m_Pages = new List<Page>();
            m_Pages = DBPage.RetrieveAll(Id);

            string Menu = "<ul id='" + Id + "' class='nav-list collapse'>";

            foreach (Page m_Page in m_Pages)
            {
                int numChildren = DBPage.getNumChildren(m_Page.PageID);

                if (numChildren > 0)
                {
                    Menu += "<li><a href='#" + m_Page.PageID + "'  data-toggle='collapse'><i class='icon-plus'></i></a>&nbsp;&nbsp;<a data-ajax='true' data-ajax-mode='replace' data-ajax-update='#linkurl' href='/Menu/getLinkUrl/" + m_Page.PageID + "'>" + m_Page.NavigationName + "</a></li>";
                    Menu += getMenu(m_Page.PageID);
                }
                else
                {
                    Menu += "<li style='padding-left: 19px;'><a data-ajax='true' data-ajax-mode='replace' data-ajax-update='#linkurl' href='/Menu/getLinkUrl/" + m_Page.PageID + "'>" + m_Page.NavigationName + "</a></li>";
                }
            }

            Menu += "</ul>";

            return Menu;
        }
    }
}