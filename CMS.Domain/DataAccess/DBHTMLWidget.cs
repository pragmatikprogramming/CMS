using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBHTMLWidget
    {
        public static void Create(HTMLWidget m_Widget)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_HTMLWidget(name, content, pageWorkFlowState) VALUES(@name, @content, 0)";
            SqlCommand insertWidget = new SqlCommand(queryString, conn);
            insertWidget.Parameters.AddWithValue("name", m_Widget.Name);
            insertWidget.Parameters.AddWithValue("content", m_Widget.Content);

            insertWidget.ExecuteNonQuery();

            conn.Close();
        }

        public static HTMLWidget RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_HTMLWidget WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getWidget = new SqlCommand(queryString, conn);
            getWidget.Parameters.AddWithValue("id", id);
            SqlDataReader widgetReader = getWidget.ExecuteReader();

            HTMLWidget m_Widget = new HTMLWidget();

            if (widgetReader.Read())
            {
                m_Widget.Id = widgetReader.GetInt32(0);
                m_Widget.Name = widgetReader.GetString(1);
                m_Widget.Content = widgetReader.GetString(2);
            }

            conn.Close();

            return m_Widget;
        }

        public static List<HTMLWidget> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_HTMLWidget WHERE pageWorkFlowState != 4";
            SqlCommand getWidgets = new SqlCommand(queryString, conn);
            SqlDataReader widgetReader = getWidgets.ExecuteReader();

            List<HTMLWidget> m_Widgets = new List<HTMLWidget>();

            while (widgetReader.Read())
            {
                HTMLWidget temp = new HTMLWidget();

                temp.Id = widgetReader.GetInt32(0);
                temp.Name = widgetReader.GetString(1);
                temp.Content = widgetReader.GetString(2);

                m_Widgets.Add(temp);
            }

            conn.Close();
            return m_Widgets;
        }

        /*public static List<HTMLWidget> RetrieveAll(int id)
        {

        }*/

        public static void Update(HTMLWidget m_Widget)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_HTMLWidget SET content = @content, name = @name WHERE id = @id";
            SqlCommand updateWidget = new SqlCommand(queryString, conn);
            updateWidget.Parameters.AddWithValue("content", m_Widget.Content);
            updateWidget.Parameters.AddWithValue("name", m_Widget.Name);
            updateWidget.Parameters.AddWithValue("id", m_Widget.Id);
            updateWidget.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            HTMLWidget m_HTMLWidget = RetrieveOne(id);

            string queryString = "UPDATE CMS_HTMLWidget SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand updateWidget = new SqlCommand(queryString, conn);
            updateWidget.Parameters.AddWithValue("id", m_HTMLWidget.Id);
            updateWidget.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_HTMLWidget', @objectName, @deleteDate, @deletedBy, 'id', 'Widget - HTML')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_HTMLWidget.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_HTMLWidget.Name);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();


            conn.Close();
        }
    }
}