using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;
using CMS.Domain.HelperClasses;


namespace CMS.Domain.Models
{
    public class PageRepository : IPageRepository
    {
        public void Create(Page m_Page)
        {
            if (m_Page.FriendlyURL == "" || m_Page.FriendlyURL == null)
            {
                m_Page.FriendlyURL = "";
            }

            DBPage.Create(m_Page);
        }

        public Page RetrieveOne(int m_Id)
        {
            Page m_Page = DBPage.RetrieveOne(m_Id);
            if (m_Page.TemplateId > 0)
            {
                m_Page.TemplateName = DBPage.GetTemplateName(m_Page.TemplateId);
            }

            return m_Page;
        }

        public Page RetrieveOneByFriendlyURL(string friendlyURL)
        {
            Page m_Page = DBPage.RetrieveOneByFriendlyURL(friendlyURL);
            if (m_Page.TemplateId > 0)
            {
                m_Page.TemplateName = DBPage.GetTemplateName(m_Page.TemplateId);
            }
            return m_Page;
        }

        public List<Page> RetrieveAll(int m_Id)
        {
            List<Page> m_Pages = DBPage.RetrieveAll(m_Id);
            return m_Pages;
        }

        public void Update(Page m_Page)
        {
            Page tempPage = DBPage.getTopByPageId(m_Page.PageID);
            m_Page.SortOrder = tempPage.SortOrder;
            m_Page.PageWorkFlowState = tempPage.PageWorkFlowState;

            if (m_Page.Id != tempPage.Id)
            {
                m_Page.Id = tempPage.Id;
            }
            if (m_Page.FriendlyURL == "" || m_Page.FriendlyURL == null)
            {
                m_Page.FriendlyURL = "";
            }

            m_Page.LockedBy = Utility.GetLockedBy(m_Page.Id);

            DBPage.Update(m_Page);
        }

        public bool TrashCan(int m_Id)
        {
            if (DBPage.TrashCan(m_Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Delete(int m_Id)
        {

        }

        public int Publish(int id)
        {
            int ParentId = DBPage.Publish(id);

            return ParentId;
        }

        public void LockPage(int pid)
        {
            DBPage.lockPage(pid);
        }

        public void UnlockPage(int pid)
        {
            DBPage.unlockPage(pid);
        }

        public void sortUp(int id)
        {
            DBPage.sortUp(id);
        }

        public void sortDown(int id)
        {
            DBPage.sortDown(id);
        }

        public bool friendlyURLExists(string friendlyURL, int pageId)
        {
            bool m_Result = DBPage.friendlyURLExists(friendlyURL, pageId);
            return m_Result;
        }
    }
}