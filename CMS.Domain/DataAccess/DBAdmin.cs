using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBAdmin
    {
        public static List<Admin> getAwaitingApproval(int pageNum)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Pages WHERE  pageWorkFlowState = 1 order by pageId, publishDate, id DESC";
            SqlCommand getPages = new SqlCommand(queryString, conn);
            SqlDataReader pageReader = getPages.ExecuteReader();

            List<Admin> m_Objects = new List<Admin>();

            int previousPageId = 0;

            while (pageReader.Read())
            {
                Admin tempAdmin = new Admin();

                tempAdmin.ObjectId = pageReader.GetInt32(0);
                tempAdmin.ObjectName = pageReader.GetString(4);
                tempAdmin.ObjectType = "Page";
                tempAdmin.ObjectTable = "CMS_Pages";
                tempAdmin.LockedBy = pageReader.GetInt32(13);
                tempAdmin.LastModifiedBy = pageReader.GetInt32(14);
                tempAdmin.LastModifiedDate = pageReader.GetDateTime(15);

                if (tempAdmin.LockedBy > 0)
                {
                    User m_User = DBUser.GetOne(tempAdmin.LockedBy);
                    tempAdmin.LockedByName = m_User.FirstName + " " + m_User.LastName;
                }
                if (tempAdmin.LastModifiedBy > 0)
                {
                    User m_User = DBUser.GetOne(tempAdmin.LastModifiedBy);
                    tempAdmin.LastModifiedName = m_User.FirstName + " " + m_User.LastName;
                }

                if(previousPageId != pageReader.GetInt32(1))
                {
                    m_Objects.Add(tempAdmin);
                }

                previousPageId = pageReader.GetInt32(1);
            }

            pageReader.Close();

            queryString = "SELECT * FROM CMS_Events WHERE pageWorkFlowState = 1";
            SqlCommand getEvent = new SqlCommand(queryString, conn);
            SqlDataReader eventReader = getEvent.ExecuteReader();

            previousPageId = 0;

            while(eventReader.Read())
            {
                Admin tempAdmin = new Admin();

                tempAdmin.ObjectId = eventReader.GetInt32(0);
                tempAdmin.ObjectName = eventReader.GetString(2);
                tempAdmin.ObjectType = "Calendar";
                tempAdmin.ObjectTable = "CMS_Events";
                tempAdmin.LockedBy = eventReader.GetInt32(8);
                tempAdmin.LastModifiedBy = eventReader.GetInt32(9);
                tempAdmin.LastModifiedDate = eventReader.GetDateTime(10);

                if(previousPageId != tempAdmin.ObjectId)
                {
                    m_Objects.Add(tempAdmin);
                }

                previousPageId = tempAdmin.ObjectId;
     
            }

            m_Objects.OrderBy(e => e.LastModifiedDate);

            conn.Close();
            return m_Objects;
        }

        public static List<Admin> getLockedContent(int pageNum)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Pages WHERE  pageWorkFlowState != 4 AND LockedBy > 0 order by pageId, publishDate, id DESC";
            SqlCommand getPages = new SqlCommand(queryString, conn);
            SqlDataReader pageReader = getPages.ExecuteReader();

            List<Admin> m_Objects = new List<Admin>();

            int previousPageId = 0;

            while (pageReader.Read())
            {
                Admin tempAdmin = new Admin();

                tempAdmin.ObjectId = pageReader.GetInt32(0);
                tempAdmin.ObjectName = pageReader.GetString(4);
                tempAdmin.ObjectType = "Page";
                tempAdmin.ObjectTable = "CMS_Pages";
                tempAdmin.LockedBy = pageReader.GetInt32(13);
                tempAdmin.LastModifiedBy = pageReader.GetInt32(14);
                tempAdmin.LastModifiedDate = pageReader.GetDateTime(15);

                if (tempAdmin.LockedBy > 0)
                {
                    User m_User = DBUser.GetOne(tempAdmin.LockedBy);
                    tempAdmin.LockedByName = m_User.FirstName + " " + m_User.LastName;
                }
                if (tempAdmin.LastModifiedBy > 0)
                {
                    User m_User = DBUser.GetOne(tempAdmin.LastModifiedBy);
                    tempAdmin.LastModifiedName = m_User.FirstName + " " + m_User.LastName;
                }

                if (previousPageId != pageReader.GetInt32(1))
                {
                    m_Objects.Add(tempAdmin);
                }

                previousPageId = pageReader.GetInt32(1);
            }

            pageReader.Close();

            queryString = "SELECT * FROM CMS_Events WHERE pageWorkFlowState != 4 AND LockedBy > 0";
            SqlCommand getEvent = new SqlCommand(queryString, conn);
            SqlDataReader eventReader = getEvent.ExecuteReader();

            previousPageId = 0;

            while (eventReader.Read())
            {
                Admin tempAdmin = new Admin();

                tempAdmin.ObjectId = eventReader.GetInt32(0);
                tempAdmin.ObjectName = eventReader.GetString(2);
                tempAdmin.ObjectType = "Calendar";
                tempAdmin.ObjectTable = "CMS_Events";
                tempAdmin.LockedBy = eventReader.GetInt32(8);
                tempAdmin.LastModifiedBy = eventReader.GetInt32(9);
                tempAdmin.LastModifiedDate = eventReader.GetDateTime(10);

                if (previousPageId != tempAdmin.ObjectId)
                {
                    m_Objects.Add(tempAdmin);
                }

                previousPageId = tempAdmin.ObjectId;

            }

            m_Objects.OrderBy(e => e.LastModifiedDate);

            conn.Close();
            return m_Objects;
        }
    }
}