using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Entities;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;

namespace CMS.Domain.DataAccess
{
    public class DBPage
    {
        //workflow state 1 == unpublished
        //workflow state 2 == published
        //workflow state 3 == expired
        //workflow state 4 == deleted non - permanent

        public static void Create(Page m_Page)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT IDENT_CURRENT('CMS_Pages')";
            SqlCommand getPageId = new SqlCommand(queryString, conn);
            int m_PageId = (int)(decimal)getPageId.ExecuteScalar();
            conn.Close();

            if (m_PageId == 1)
            {
                conn.Open();
                queryString = "SELECT COUNT(*) FROM CMS_Pages";
                SqlCommand getPageCount = new SqlCommand(queryString, conn);
                int pageCount = (int)getPageCount.ExecuteScalar();

                if (m_PageId == pageCount)
                {
                    m_PageId = pageCount + 1;
                }
                conn.Close();
            }
            else
            {
                m_PageId++;
            }

            conn.Open();

            queryString = "INSERT INTO CMS_Pages(pageId, contentGroup, templateId, pageTitle, navigationName, publishDate, expireDate, content, metaDescription, metaKeywords, parentId, pageWorkFlowState, lockedBy, lastModifiedBy, lastModifiedDate) VALUES(@pageId, @contentGroup, @templateId, @pageTitle, @navigationName, @publishDate, @expireDate, @content, @metaDescription, @metaKeywords, @parentId, 1, @lockedBy, @lastModifiedBy, @lastModifiedDate)";
            SqlCommand insertPage = new SqlCommand(queryString, conn);
            insertPage.Parameters.AddWithValue("pageId", m_PageId);
            insertPage.Parameters.AddWithValue("contentGroup", m_Page.ContentGroup);
            insertPage.Parameters.AddWithValue("templateId", m_Page.TemplateId);
            insertPage.Parameters.AddWithValue("pageTitle", m_Page.PageTitle);
            insertPage.Parameters.AddWithValue("navigationName", m_Page.NavigationName);
            insertPage.Parameters.AddWithValue("publishDate", m_Page.PublishDate.ToString());
            insertPage.Parameters.AddWithValue("expireDate", m_Page.ExpireDate.ToString());
            insertPage.Parameters.AddWithValue("content", m_Page.Content ?? string.Empty);
            insertPage.Parameters.AddWithValue("metaDescription", m_Page.MetaDescription ?? string.Empty);
            insertPage.Parameters.AddWithValue("metaKeywords", m_Page.MetaKeywords ?? string.Empty);
            insertPage.Parameters.AddWithValue("parentId", m_Page.ParentId);
            insertPage.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
            insertPage.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
            insertPage.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
            insertPage.ExecuteNonQuery();

            conn.Close();
        }

        public static Page RetrieveOne(int m_Id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            string action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            if (action == "PagePreview")
            {
                queryString = "SELECT * FROM CMS_Pages WHERE id = @id";
            }
            else
            {
                queryString = "SELECT * FROM CMS_Pages WHERE id = @id AND pageWorkFlowState != 4";
            }
            SqlCommand getPages = new SqlCommand(queryString, conn);
            getPages.Parameters.AddWithValue("id", m_Id);
            SqlDataReader pageDataReader = getPages.ExecuteReader();

            Page m_Page = new Page();

            if (pageDataReader.Read())
            {
                m_Page.Id = pageDataReader.GetInt32(0);
                m_Page.PageID = pageDataReader.GetInt32(1);
                m_Page.ContentGroup = pageDataReader.GetInt32(2);
                m_Page.TemplateId = pageDataReader.GetInt32(3);
                m_Page.PageTitle = pageDataReader.GetString(4);
                m_Page.NavigationName = pageDataReader.GetString(5);
                m_Page.PublishDate = pageDataReader.GetDateTime(6);
                m_Page.ExpireDate = pageDataReader.GetDateTime(7);
                m_Page.Content = pageDataReader.GetString(8);
                m_Page.MetaDescription = pageDataReader.GetString(9);
                m_Page.MetaKeywords = pageDataReader.GetString(10);
                m_Page.ParentId = pageDataReader.GetInt32(11);
                m_Page.PageWorkFlowState = pageDataReader.GetInt32(12);
                m_Page.LockedBy = pageDataReader.GetInt32(13);
                m_Page.LockedByName = DBPage.GetLockedByName(m_Page.LockedBy);
            }

            conn.Close();
            return m_Page;
        }

        public static List<Page> RetrieveAll(int m_Id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Pages WHERE parentId = @parentId AND pageWorkFlowState != 4 order by pageId, publishDate, id DESC";
            SqlCommand getPages = new SqlCommand(queryString, conn);
            getPages.Parameters.AddWithValue("parentId", m_Id);
            SqlDataReader pagesDataReader = getPages.ExecuteReader();

            List<Page> m_Pages = new List<Page>();
            int previousPageId = 0;

            while (pagesDataReader.Read())
            {
                Page tempPage = new Page();

                tempPage.Id = pagesDataReader.GetInt32(0);
                tempPage.PageID = pagesDataReader.GetInt32(1);
                tempPage.ContentGroup = pagesDataReader.GetInt32(2);
                tempPage.TemplateId = pagesDataReader.GetInt32(3);
                tempPage.PageTitle = pagesDataReader.GetString(4);
                tempPage.NavigationName = pagesDataReader.GetString(5);
                tempPage.PublishDate = pagesDataReader.GetDateTime(6);
                tempPage.ExpireDate = pagesDataReader.GetDateTime(7);
                tempPage.Content = pagesDataReader.GetString(8);
                tempPage.MetaDescription = pagesDataReader.GetString(9);
                tempPage.MetaKeywords = pagesDataReader.GetString(10);
                tempPage.ParentId = pagesDataReader.GetInt32(11);
                tempPage.PageWorkFlowState = pagesDataReader.GetInt32(12);
                tempPage.LockedBy = pagesDataReader.GetInt32(13);
                tempPage.LockedByName = DBPage.GetLockedByName(tempPage.LockedBy);

                if (previousPageId != tempPage.PageID)
                {
                    m_Pages.Add(tempPage);
                }

                previousPageId = tempPage.PageID;

            }

            conn.Close();
            return m_Pages;

        }

        public static void Update(Page m_Page)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            if (m_Page.PageWorkFlowState == 1)
            {
                string queryString = "UPDATE CMS_Pages SET contentGroup = @contentGroup, templateId = @templateId, pageTitle = @pageTitle, navigationName = @navigationName, publishDate = @publishDate, expireDate = @expireDate, content = @content, metaDescription = @metaDescription, metaKeywords = @metaKeywords, parentId = @parentId, pageWorkFlowState = 1, lockedBy = @lockedBy, lastModifiedBy = @lastModifiedBy, lastModifiedDate = @lastModifiedDate WHERE id = @id and pageId = @pageId";
                SqlCommand updatePage = new SqlCommand(queryString, conn);

                updatePage.Parameters.AddWithValue("contentGroup", m_Page.ContentGroup);
                updatePage.Parameters.AddWithValue("templateId", m_Page.TemplateId);
                updatePage.Parameters.AddWithValue("pageTitle", m_Page.PageTitle);
                updatePage.Parameters.AddWithValue("navigationName", m_Page.NavigationName);
                updatePage.Parameters.AddWithValue("publishDate", m_Page.PublishDate.ToString());
                updatePage.Parameters.AddWithValue("expireDate", m_Page.ExpireDate.ToString());
                updatePage.Parameters.AddWithValue("content", m_Page.Content ?? string.Empty);
                updatePage.Parameters.AddWithValue("metaDescription", m_Page.MetaDescription ?? string.Empty);
                updatePage.Parameters.AddWithValue("metaKeywords", m_Page.MetaKeywords ?? string.Empty);
                updatePage.Parameters.AddWithValue("parentId", m_Page.ParentId);
                updatePage.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
                updatePage.Parameters.AddWithValue("id", m_Page.Id);
                updatePage.Parameters.AddWithValue("PageId", m_Page.PageID);
                updatePage.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
                updatePage.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);

                updatePage.ExecuteNonQuery();
            }
            else if (m_Page.PageWorkFlowState == 2 || m_Page.PageWorkFlowState == 3)
            {
                string queryString = "INSERT INTO CMS_Pages(pageId, contentGroup, templateId, pageTitle, navigationName, publishDate, expireDate, content, metaDescription, metaKeywords, parentId, pageWorkFlowState, lockedBy, lastModifiedBy = @lastModifiedBy, lastModifiedDate = @lastModifiedDate) VALUES(@pageId, @contentGroup, @templateId, @pageTitle, @navigationName, @publishDate, @expireDate, @content, @metaDescription, @metaKeywords, @parentId, 1, @lockedBy)";
                SqlCommand insertPage = new SqlCommand(queryString, conn);
                insertPage.Parameters.AddWithValue("pageId", m_Page.PageID);
                insertPage.Parameters.AddWithValue("contentGroup", m_Page.ContentGroup);
                insertPage.Parameters.AddWithValue("templateId", m_Page.TemplateId);
                insertPage.Parameters.AddWithValue("pageTitle", m_Page.PageTitle);
                insertPage.Parameters.AddWithValue("navigationName", m_Page.NavigationName);
                insertPage.Parameters.AddWithValue("publishDate", m_Page.PublishDate.ToString());
                insertPage.Parameters.AddWithValue("expireDate", m_Page.ExpireDate.ToString());
                insertPage.Parameters.AddWithValue("content", m_Page.Content ?? string.Empty);
                insertPage.Parameters.AddWithValue("metaDescription", m_Page.MetaDescription ?? string.Empty);
                insertPage.Parameters.AddWithValue("metaKeywords", m_Page.MetaKeywords ?? string.Empty);
                insertPage.Parameters.AddWithValue("parentId", m_Page.ParentId);
                insertPage.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
                insertPage.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
                insertPage.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
                insertPage.ExecuteNonQuery();
            }
            else
            {

            }

            conn.Close();
        }

        public static bool TrashCan(int m_ID)
        {
            Page m_Page = DBPage.RetrieveOne(m_ID);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT COUNT(*) FROM CMS_Pages WHERE parentId = @parentId";
            SqlCommand getNumChildren = new SqlCommand(queryString, conn);
            getNumChildren.Parameters.AddWithValue("parentId", m_Page.PageID);
            int numChildren = (int)getNumChildren.ExecuteScalar();

            

            if (numChildren == 0)
            {
                queryString = "UPDATE CMS_Pages SET pageWorkFlowState = 4 WHERE pageId = @pageId";
                SqlCommand updatePage = new SqlCommand(queryString, conn);
                updatePage.Parameters.AddWithValue("pageId", m_Page.PageID);
                updatePage.ExecuteNonQuery();

                queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_Pages', @objectName, @deleteDate, @deletedBy, 'pageId', 'Page')";
                SqlCommand insertTrash = new SqlCommand(queryString, conn);
                insertTrash.Parameters.AddWithValue("objectId", m_Page.PageID);
                insertTrash.Parameters.AddWithValue("objectName", m_Page.PageTitle);
                insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
                insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
                insertTrash.ExecuteNonQuery();
            }
            else
            {
                return false;
            }

            conn.Close();
            return true;
        }

        public void Delete(int m_ID)
        {

        }

        public static int Publish(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT parentId FROM CMS_Pages WHERE id = @id";
            SqlCommand getPID = new SqlCommand(queryString, conn);
            getPID.Parameters.AddWithValue("id", id);
            int parentId = (int)getPID.ExecuteScalar();

            queryString = "UPDATE CMS_Pages set pageWorkFlowState = 2 WHERE id = @id";
            SqlCommand updateWFS = new SqlCommand(queryString, conn);
            updateWFS.Parameters.AddWithValue("id", id);
            updateWFS.ExecuteNonQuery();

            conn.Close();

            return parentId;
        }

        public static string GetTemplateName(int templateId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT templateName from CMS_Templates WHERE templateId = @templateId";
            SqlCommand getTemplateName = new SqlCommand(queryString, conn);
            getTemplateName.Parameters.AddWithValue("templateId", templateId);
            string templateName = getTemplateName.ExecuteScalar().ToString() ?? string.Empty;

            conn.Close();

            return templateName;
        }

        public static string GetLockedByName(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT firstName, lastName FROM CMS_Users WHERE id = @id";
            SqlCommand getLB = new SqlCommand(queryString, conn);
            getLB.Parameters.AddWithValue("id", id);
            SqlDataReader getLockedByReader = getLB.ExecuteReader();

            string m_Name = "";

            if (getLockedByReader.Read())
            {
                m_Name = getLockedByReader.GetString(0);
                m_Name += " ";
                m_Name += getLockedByReader.GetString(1);
            }

            conn.Close();

            return m_Name;
        }

        public static void lockPage(int m_pid)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Pages SET lockedBy = @lockedBy WHERE id = @id";
            SqlCommand m_LockPage = new SqlCommand(queryString, conn);
            m_LockPage.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
            m_LockPage.Parameters.AddWithValue("id", m_pid);
            m_LockPage.ExecuteNonQuery();

            conn.Close();
        }

        public static void unlockPage(int m_pid)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Pages SET lockedBy = 0 WHERE id = @id";
            SqlCommand m_LockPage = new SqlCommand(queryString, conn);
            m_LockPage.Parameters.AddWithValue("id", m_pid);
            m_LockPage.ExecuteNonQuery();

            conn.Close();
        }
    }
}