using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.Entities;
using System.Collections.Specialized;
using System.Configuration;

namespace CMS.Domain.HelperClasses
{
    public class Utility
    {
        public static List<ContentGroup> ContentGroups()
        {
            List<ContentGroup> m_ContentGroups = new List<ContentGroup>();

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_ContentGroups WHERE id != 1 ORDER BY ContentGroup";
            SqlCommand getContentGroups = new SqlCommand(queryString, conn);

            SqlDataReader ContentGroups = getContentGroups.ExecuteReader();

            while (ContentGroups.Read())
            {
                ContentGroup m_ContentGroup = new ContentGroup();
                m_ContentGroup.GroupID = ContentGroups.GetInt32(0);
                m_ContentGroup.ContentGroupName = ContentGroups.GetString(1);
                m_ContentGroups.Add(m_ContentGroup);
            }

            conn.Close();
            return m_ContentGroups;
        }

        public static List<Branch> BranchNames()
        {
            List<Branch> m_Branchs = new List<Branch>();

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_BranchNames ORDER BY BranchName";
            SqlCommand getBranchNames = new SqlCommand(queryString, conn);

            SqlDataReader BranchNames = getBranchNames.ExecuteReader();

            while (BranchNames.Read())
            {
                Branch m_Branch = new Branch();
                m_Branch.Id = BranchNames.GetInt32(0);
                m_Branch.BranchName = BranchNames.GetString(1);
                m_Branchs.Add(m_Branch);
            }

            conn.Close();
            return m_Branchs;
        }

        public static bool mimeTypeAllowed(string mimeType)
        {
            NameValueCollection m_MimeTypes = (NameValueCollection)ConfigurationManager.GetSection("allowedMimeTypes");

            if(!String.IsNullOrEmpty(m_MimeTypes[mimeType]))
            {
                return true;
            }

            return false;
        }

        public static string GetTemplateById(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT templateName FROM CMS_Templates WHERE templateId = @id";
            SqlCommand getTemplate = new SqlCommand(queryString, conn);
            getTemplate.Parameters.AddWithValue("id", id);
            string templateName = (string)getTemplate.ExecuteScalar();

            return templateName;
        }

        public static List<Template> GetTemplates()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Templates";
            SqlCommand getTemplates = new SqlCommand(queryString, conn);
            SqlDataReader templateReader = getTemplates.ExecuteReader();

            List<Template> m_Templates = new List<Template>();

            while (templateReader.Read())
            {
                Template temp = new Template();
                temp.TemplateId = templateReader.GetInt32(0);
                temp.TemplateName = templateReader.GetString(1);
                temp.FriendlyName = templateReader.GetString(2);

                m_Templates.Add(temp);
            }

            conn.Close();

            return m_Templates;            
        }

        public static int GetPageWorkFlowStatus(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT pageWorkFlowState FROM CMS_Pages WHERE id = @id";
            SqlCommand getWFS = new SqlCommand(queryString, conn);
            getWFS.Parameters.AddWithValue("id", id);
            int WFS = (int)getWFS.ExecuteScalar();

            return WFS;
        }

        public static int GetLockedBy(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT lockedBy FROM CMS_Pages WHERE id = @id";
            SqlCommand getLB = new SqlCommand(queryString, conn);
            getLB.Parameters.AddWithValue("id", id);
            int LB = (int)getLB.ExecuteScalar();

            return LB;
        }

        public static string getBranchName(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT BranchName FROM CMS_BranchNames WHERE id = @id";
            SqlCommand getBranchName = new SqlCommand(queryString, conn);
            getBranchName.Parameters.AddWithValue("id", id);
            string branchName = (string)getBranchName.ExecuteScalar();

            conn.Close();

            return branchName;
        }

        public static string getMenuName(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT menuName from CMS_Menus WHERE id = @id";
            SqlCommand getMenuName = new SqlCommand(queryString, conn);
            getMenuName.Parameters.AddWithValue("id", id);
            string menuName = (string)getMenuName.ExecuteScalar();

            conn.Close();

            return menuName;
        }
    }
}