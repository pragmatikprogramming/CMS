using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;
using CMS.Domain.Entities;

namespace CMS.Domain.DataAccess
{
    public class DBHome
    {
        public static List<Page> MainMenu()
        {
            List<Page> m_Pages = DBPage.RetrieveAll(0);
            return m_Pages;
        }

        public static void SubmitComment(BlogPostComment m_Comment)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_BlogPostComments(comment, name, blogId, pageWorkFlowState) VALUES(@comment, @name, @blogId, 2)";
            SqlCommand insertComment = new SqlCommand(queryString, conn);
            insertComment.Parameters.AddWithValue("comment", m_Comment.Comment);
            insertComment.Parameters.AddWithValue("name", m_Comment.Name);
            insertComment.Parameters.AddWithValue("blogId", m_Comment.BlogId);
            insertComment.ExecuteNonQuery();

            conn.Close();
        }

        
    }
}