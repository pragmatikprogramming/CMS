using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBWidgetContainer
    {
        public static int Create(WidgetContainer m_Container)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_WidgetContainers(name) VALUES(@name)";
            SqlCommand insertContainer = new SqlCommand(queryString, conn);
            insertContainer.Parameters.AddWithValue("name", m_Container.Name);
            insertContainer.ExecuteNonQuery();

            queryString = "SELECT IDENT_CURRENT('CMS_WidgetContainers')";
            SqlCommand getPageId = new SqlCommand(queryString, conn);
            int m_ContainerId = (int)(decimal)getPageId.ExecuteScalar();

            conn.Close();

            foreach (int widget in m_Container.MyWidgets)
            {
                conn.Open();

                int sortOrder = getSortOrder(m_ContainerId);

                queryString = "INSERT INTO CMS_ContainerToWidgets(containerId, widgetId, sortOrder) VALUES(@containerId, @widgetId, @sortOrder)";
                SqlCommand insertInfo = new SqlCommand(queryString, conn);
                insertInfo.Parameters.AddWithValue("containerId", m_ContainerId);
                insertInfo.Parameters.AddWithValue("widgetId", widget);
                insertInfo.Parameters.AddWithValue("sortOrder", sortOrder);

                insertInfo.ExecuteNonQuery();

                conn.Close();
            }

            return m_ContainerId;
        }

        public static WidgetContainer RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_WidgetContainers WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getContainer = new SqlCommand(queryString, conn);
            getContainer.Parameters.AddWithValue("id", id);
            SqlDataReader containerReader = getContainer.ExecuteReader();

            WidgetContainer m_Container = new WidgetContainer();

            if(containerReader.Read())
            {
                m_Container.Id = containerReader.GetInt32(0);
                m_Container.Name = containerReader.GetString(1);

                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                queryString = "select w.id, w.name, w.content, ctw.sortOrder from CMS_ContainerToWidgets ctw, CMS_HTMLWidget w WHERE ctw.widgetId = w.id AND ctw.containerId = @id AND w.pageWorkFlowState != 4 ORDER BY ctw.sortOrder ASC";
                SqlCommand getWidgets = new SqlCommand(queryString, conn2);
                getWidgets.Parameters.AddWithValue("id", m_Container.Id);
                SqlDataReader widgetReader = getWidgets.ExecuteReader();

                m_Container.Widgets = new List<HTMLWidget>();
                m_Container.MyWidgets = new List<int>();

                while (widgetReader.Read())
                {
                    HTMLWidget m_Widget = new HTMLWidget();

                    m_Widget.Id = widgetReader.GetInt32(0);
                    m_Widget.Name = widgetReader.GetString(1);
                    m_Widget.Content = widgetReader.GetString(2);
                    m_Widget.SortOrder = widgetReader.GetInt32(3);

                    m_Container.MyWidgets.Add(widgetReader.GetInt32(0));
                    m_Container.Widgets.Add(m_Widget);
                }

                conn2.Close();
            }

            conn.Close();

            return m_Container;
        }

        public static WidgetContainer RetrieveOneByName(string name)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_WidgetContainers WHERE name = @name AND pageWorkFlowState != 4";
            SqlCommand getContainer = new SqlCommand(queryString, conn);
            getContainer.Parameters.AddWithValue("name", name);
            SqlDataReader containerReader = getContainer.ExecuteReader();

            WidgetContainer m_Container = new WidgetContainer();

            if (containerReader.Read())
            {
                m_Container.Id = containerReader.GetInt32(0);
                m_Container.Name = containerReader.GetString(1);

                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                queryString = "select w.id, w.name, w.content, ctw.sortOrder from CMS_ContainerToWidgets ctw, CMS_HTMLWidget w WHERE ctw.widgetId = w.id AND ctw.containerId = @id AND w.pageWorkFlowState != 4 ORDER BY ctw.sortOrder ASC";
                SqlCommand getWidgets = new SqlCommand(queryString, conn2);
                getWidgets.Parameters.AddWithValue("id", m_Container.Id);
                SqlDataReader widgetReader = getWidgets.ExecuteReader();

                m_Container.Widgets = new List<HTMLWidget>();
                m_Container.MyWidgets = new List<int>();

                while (widgetReader.Read())
                {
                    HTMLWidget m_Widget = new HTMLWidget();

                    m_Widget.Id = widgetReader.GetInt32(0);
                    m_Widget.Name = widgetReader.GetString(1);
                    m_Widget.Content = widgetReader.GetString(2);
                    m_Widget.SortOrder = widgetReader.GetInt32(3);

                    m_Container.MyWidgets.Add(widgetReader.GetInt32(0));
                    m_Container.Widgets.Add(m_Widget);
                }

                conn2.Close();
            }

            conn.Close();

            return m_Container;
        }

        public static WidgetContainer RetrieveOneByTemplateId(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_WidgetContainers WHERE templateId = @id AND pageWorkFlowState != 4";
            SqlCommand getContainer = new SqlCommand(queryString, conn);
            getContainer.Parameters.AddWithValue("id", id);
            SqlDataReader containerReader = getContainer.ExecuteReader();

            WidgetContainer m_Container = new WidgetContainer();

            if (containerReader.Read())
            {
                m_Container.Id = containerReader.GetInt32(0);
                m_Container.Name = containerReader.GetString(1);

                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                queryString = "select w.id, w.name, w.content, ctw.sortOrder from CMS_ContainerToWidgets ctw, CMS_HTMLWidget w WHERE ctw.widgetId = w.id AND ctw.containerId = @id AND w.pageWorkFlowState != 4 ORDER BY ctw.sortOrder ASC";
                SqlCommand getWidgets = new SqlCommand(queryString, conn2);
                getWidgets.Parameters.AddWithValue("id", m_Container.Id);
                SqlDataReader widgetReader = getWidgets.ExecuteReader();

                m_Container.Widgets = new List<HTMLWidget>();
                m_Container.MyWidgets = new List<int>();
            
                while (widgetReader.Read())
                {
                    HTMLWidget m_Widget = new HTMLWidget();

                    m_Widget.Id = widgetReader.GetInt32(0);
                    m_Widget.Name = widgetReader.GetString(1);
                    m_Widget.Content = widgetReader.GetString(2);
                    m_Widget.SortOrder = widgetReader.GetInt32(3);

                    m_Container.MyWidgets.Add(widgetReader.GetInt32(0));
                    m_Container.Widgets.Add(m_Widget);
                }

                conn2.Close();
            }

            conn.Close();

            return m_Container;
        }

        public static List<WidgetContainer> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_WidgetContainers WHERE pageWorkFlowState != 4";
            SqlCommand getContainer = new SqlCommand(queryString, conn);
            SqlDataReader containerReader = getContainer.ExecuteReader();

            List<WidgetContainer> m_Containers = new List<WidgetContainer>();

            while (containerReader.Read())
            {
                WidgetContainer m_Container = new WidgetContainer();
                m_Container.Id = containerReader.GetInt32(0);
                m_Container.Name = containerReader.GetString(1);
                m_Containers.Add(m_Container);

                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                queryString = "select w.id, w.name, w.content, ctw.sortOrder from CMS_ContainerToWidgets ctw, CMS_HTMLWidget w WHERE ctw.widgetId = w.id AND ctw.containerId = 1 AND w.pageWorkFlowState != 4 ORDER BY ctw.sortOrder ASC";
                SqlCommand getWidgets = new SqlCommand(queryString, conn2);
                SqlDataReader widgetReader = getWidgets.ExecuteReader();

                m_Container.Widgets = new List<HTMLWidget>();
                m_Container.MyWidgets = new List<int>();

                while (widgetReader.Read())
                {
                    HTMLWidget m_Widget = new HTMLWidget();

                    m_Widget.Id = widgetReader.GetInt32(0);
                    m_Widget.Name = widgetReader.GetString(1);
                    m_Widget.Content = widgetReader.GetString(2);
                    m_Widget.SortOrder = widgetReader.GetInt32(3);

                    m_Container.MyWidgets.Add(widgetReader.GetInt32(0));
                    m_Container.Widgets.Add(m_Widget);
                }

                conn2.Close();
            }

            conn.Close();

            return m_Containers;
        }

        public static List<HTMLWidget> RetrieveAll(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "select w.id, w.name, w.content, ctw.sortOrder from CMS_ContainerToWidgets ctw, CMS_HTMLWidget w WHERE ctw.widgetId = w.id AND  w.pageWorkFlowState != 4 AND ctw.containerID = @id ORDER BY ctw.sortOrder ASC";
            SqlCommand getWidgets = new SqlCommand(queryString, conn);
            getWidgets.Parameters.AddWithValue("id", id);
            SqlDataReader widgetReader = getWidgets.ExecuteReader();

            List<HTMLWidget> m_Widgets = new List<HTMLWidget>();

            while (widgetReader.Read())
            {
                HTMLWidget m_Widget = new HTMLWidget();

                m_Widget.Id = widgetReader.GetInt32(0);
                m_Widget.Name = widgetReader.GetString(1);
                m_Widget.Content = widgetReader.GetString(2);
                m_Widget.SortOrder = widgetReader.GetInt32(3);

                m_Widgets.Add(m_Widget);
            }

            conn.Close();

            return m_Widgets;
        }

        public static void Update(WidgetContainer m_Container)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_WidgetContainers SET name = @name WHERE id = @id";
            SqlCommand updateContainer = new SqlCommand(queryString, conn);
            updateContainer.Parameters.AddWithValue("name", m_Container.Name);
            updateContainer.Parameters.AddWithValue("id", m_Container.Id);
            updateContainer.ExecuteNonQuery();

            queryString = "DELETE FROM CMS_ContainerToWidgets WHERE containerId = @containerId";
            SqlCommand deleteBridge = new SqlCommand(queryString, conn);
            deleteBridge.Parameters.AddWithValue("containerId", m_Container.Id);
            deleteBridge.ExecuteNonQuery();

            conn.Close();

            foreach (int widget in m_Container.MyWidgets)
            {
                conn.Open();

                int sortOrder = getSortOrder(m_Container.Id);

                queryString = "INSERT INTO CMS_ContainerToWidgets(containerId, widgetId, sortOrder) VALUES(@containerId, @widgetId, @sortOrder)";
                SqlCommand insertInfo = new SqlCommand(queryString, conn);
                insertInfo.Parameters.AddWithValue("containerId", m_Container.Id);
                insertInfo.Parameters.AddWithValue("widgetId", widget);
                insertInfo.Parameters.AddWithValue("sortOrder", sortOrder);

                insertInfo.ExecuteNonQuery();

                conn.Close();
            }

        }

        public static void Delete(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            WidgetContainer m_Container = RetrieveOne(id);

            string queryString = "UPDATE CMS_WidgetContainers SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand updateWidget = new SqlCommand(queryString, conn);
            updateWidget.Parameters.AddWithValue("id", m_Container.Id);
            updateWidget.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_WidgetContainers', @objectName, @deleteDate, @deletedBy, 'id', 'Widget Container')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_Container.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_Container.Name);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();

            conn.Close();
        }

        public static List<HTMLWidget> getWidgets()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_HTMLWidget";
            SqlCommand getWidgets = new SqlCommand(queryString, conn);
            SqlDataReader widgetReader = getWidgets.ExecuteReader();

            List<HTMLWidget> m_Widgets = new List<HTMLWidget>();

            while (widgetReader.Read())
            {
                HTMLWidget temp = new HTMLWidget();

                temp.Id = widgetReader.GetInt32(0);
                temp.Name = widgetReader.GetString(1);

                m_Widgets.Add(temp);
            }

            conn.Close();

            return m_Widgets;
        }

        public static int getSortOrder(int containerId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 sortOrder FROM CMS_ContainerToWidgets WHERE containerId = @containerId ORDER BY sortOrder DESC";
            SqlCommand getSortOrder = new SqlCommand(queryString, conn);
            getSortOrder.Parameters.AddWithValue("containerId", containerId);
            object sortOrder = getSortOrder.ExecuteScalar();
            int m_sortOrder = 0;

            if (sortOrder == null)
            {
                m_sortOrder = 1;
            }
            else
            {
                m_sortOrder = Convert.ToInt32(sortOrder);
                m_sortOrder++;
            }

            conn.Close();

            return m_sortOrder;
        }

        public static void SortUp(int parentId, int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 sortOrder FROM CMS_ContainerToWidgets WHERE containerId = @parentId AND widgetId = @id";
            SqlCommand getSortOrder = new SqlCommand(queryString, conn);
            getSortOrder.Parameters.AddWithValue("parentId", parentId);
            getSortOrder.Parameters.AddWithValue("id", id);

            int oldSortOrder = (int)getSortOrder.ExecuteScalar();
            int newSortOrder = oldSortOrder - 1;

            if (newSortOrder > 0)
            {
                queryString = "UPDATE CMS_ContainerToWidgets SET sortOrder = @sortOrder WHERE containerId = @parentId AND sortOrder = @newSortOrder";
                SqlCommand updateSort = new SqlCommand(queryString, conn);
                updateSort.Parameters.AddWithValue("sortOrder", oldSortOrder);
                updateSort.Parameters.AddWithValue("parentId", parentId);
                updateSort.Parameters.AddWithValue("newSortOrder", newSortOrder);
                updateSort.ExecuteNonQuery();

                queryString = "UPDATE CMS_ContainerToWidgets SET sortOrder = @sortOrder WHERE containerId = @parentId AND widgetId = @id";
                SqlCommand updateSort2 = new SqlCommand(queryString, conn);
                updateSort2.Parameters.AddWithValue("sortOrder", newSortOrder);
                updateSort2.Parameters.AddWithValue("parentId", parentId);
                updateSort2.Parameters.AddWithValue("id", id);
                updateSort2.ExecuteNonQuery();
            }
        }

        public static void SortDown(int parentId, int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 sortOrder FROM CMS_ContainerToWidgets WHERE containerId = @parentId AND widgetId = @id";
            SqlCommand getSortOrder = new SqlCommand(queryString, conn);
            getSortOrder.Parameters.AddWithValue("parentId", parentId);
            getSortOrder.Parameters.AddWithValue("id", id);

            int oldSortOrder = (int)getSortOrder.ExecuteScalar();
            int newSortOrder = oldSortOrder + 1;


            // check boundaries and sort order integrity
            queryString = "SELECT COUNT(*) FROM CMS_ContainerToWidgets WHERE sortOrder = @sortOrder and containerId = @parentId";
            SqlCommand checkBoundaries = new SqlCommand(queryString, conn);
            checkBoundaries.Parameters.AddWithValue("sortOrder", newSortOrder);
            checkBoundaries.Parameters.AddWithValue("parentId", parentId);

            int m_Count = (int)checkBoundaries.ExecuteScalar();

            if (m_Count == 1)
            {
                queryString = "UPDATE CMS_ContainerToWidgets SET sortOrder = @sortOrder WHERE containerId = @parentId AND sortOrder = @newSortOrder";
                SqlCommand updateSort = new SqlCommand(queryString, conn);
                updateSort.Parameters.AddWithValue("sortOrder", oldSortOrder);
                updateSort.Parameters.AddWithValue("parentId", parentId);
                updateSort.Parameters.AddWithValue("newSortOrder", newSortOrder);
                updateSort.ExecuteNonQuery();

                queryString = "UPDATE CMS_ContainerToWidgets SET sortOrder = @sortOrder WHERE containerId = @parentId AND widgetId = @id";
                SqlCommand updateSort2 = new SqlCommand(queryString, conn);
                updateSort2.Parameters.AddWithValue("sortOrder", newSortOrder);
                updateSort2.Parameters.AddWithValue("parentId", parentId);
                updateSort2.Parameters.AddWithValue("id", id);
                updateSort2.ExecuteNonQuery();
            }
        }
    }
}