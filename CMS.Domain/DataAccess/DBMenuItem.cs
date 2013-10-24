using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;

namespace CMS.Domain.DataAccess
{
    public class DBMenuItem
    {
        public static void Create(MenuItem m_MenuItem)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            int sortOrder = getSortOrder(m_MenuItem.ParentId);

            string queryString = "INSERT INTO CMS_MenuItems(parentId, menuItemName, linkUrl, pageWorkFlowState, sortOrder) VALUES(@parentId, @menuItemName, @linkUrl, 0, @sortOrder)";
            SqlCommand insertMenuItem = new SqlCommand(queryString, conn);
            insertMenuItem.Parameters.AddWithValue("parentId", m_MenuItem.ParentId);
            insertMenuItem.Parameters.AddWithValue("menuItemName", m_MenuItem.MenuItemName);
            insertMenuItem.Parameters.AddWithValue("linkUrl", m_MenuItem.LinkUrl);
            insertMenuItem.Parameters.AddWithValue("sortOrder", sortOrder);
            insertMenuItem.ExecuteNonQuery();

            conn.Close();
        }

        public static MenuItem RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_MenuItems WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getMenuItem = new SqlCommand(queryString, conn);
            getMenuItem.Parameters.AddWithValue("id", id);
            SqlDataReader menuDateReader = getMenuItem.ExecuteReader();

            MenuItem m_MenuItem = new MenuItem();

            if (menuDateReader.Read())
            {
                m_MenuItem.Id = menuDateReader.GetInt32(0);
                m_MenuItem.ParentId = menuDateReader.GetInt32(1);
                m_MenuItem.MenuItemName = menuDateReader.GetString(2);
                m_MenuItem.LinkUrl = menuDateReader.GetString(3);
                m_MenuItem.PageWorkFlowState = menuDateReader.GetInt32(4);
                m_MenuItem.SortOrder = menuDateReader.GetInt32(5);
            }

            conn.Close();
            return m_MenuItem;
        }

        public static List<MenuItem> RetrieveAll(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_MenuItems WHERE parentId = @parentId AND pageWorkFlowState != 4 ORDER BY sortOrder";
            SqlCommand getMenuItems = new SqlCommand(queryString, conn);
            getMenuItems.Parameters.AddWithValue("parentId", id);
            SqlDataReader menuDataReader = getMenuItems.ExecuteReader();

            List<MenuItem> m_MenuItems = new List<MenuItem>();

            while (menuDataReader.Read())
            {
                MenuItem tempMenuItem = new MenuItem();
                tempMenuItem.Id = menuDataReader.GetInt32(0);
                tempMenuItem.ParentId = menuDataReader.GetInt32(1);
                tempMenuItem.MenuItemName = menuDataReader.GetString(2);
                tempMenuItem.LinkUrl = menuDataReader.GetString(3);
                tempMenuItem.PageWorkFlowState = menuDataReader.GetInt32(4);

                m_MenuItems.Add(tempMenuItem);
            }

            conn.Close();
            return m_MenuItems;
        }

        public static void Update(MenuItem m_MenuItem)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_MenuItems SET menuItemName = @menuItemName, linkUrl = @linkUrl WHERE id = @id";
            SqlCommand updateMenuItem = new SqlCommand(queryString, conn);
            updateMenuItem.Parameters.AddWithValue("menuItemName", m_MenuItem.MenuItemName);
            updateMenuItem.Parameters.AddWithValue("linkUrl", m_MenuItem.LinkUrl);
            updateMenuItem.Parameters.AddWithValue("id", m_MenuItem.Id);
            updateMenuItem.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            MenuItem m_MenuItem = DBMenuItem.RetrieveOne(id);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_MenuItems SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand updateMenu = new SqlCommand(queryString, conn);
            updateMenu.Parameters.AddWithValue("id", id);
            updateMenu.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_MenuItems', @objectName, @deleteDate, @deletedBy, 'id', 'Menu Item')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_MenuItem.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_MenuItem.MenuItemName);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();

            conn.Close();
        }

        public static int getSortOrder(int parentId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 sortOrder FROM CMS_MenuItems WHERE parentId = @parentId ORDER BY sortOrder DESC";
            SqlCommand getSortOrder = new SqlCommand(queryString, conn);
            getSortOrder.Parameters.AddWithValue("parentId", parentId);
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

        public static void sortUp(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            MenuItem m_MenuItem = DBMenuItem.RetrieveOne(id);
            int oldSortOrder = m_MenuItem.SortOrder;
            int newSortOrder = oldSortOrder - 1;

            //check boundaries of sort to make sure they are valid

            string queryString = "SELECT TOP 1 id FROM CMS_MenuItems WHERE parentId = @parentId and sortOrder = @sortOrder ORDER BY id DESC";
            SqlCommand getId = new SqlCommand(queryString, conn);
            getId.Parameters.AddWithValue("parentId", m_MenuItem.ParentId);
            getId.Parameters.AddWithValue("sortOrder", newSortOrder);

            object myId = getId.ExecuteScalar();

            if (myId != null)
            {
                int m_id = Convert.ToInt32(myId);
                MenuItem o_MenuItem = DBMenuItem.RetrieveOne(m_id);

                queryString = "UPDATE CMS_MenuItems SET sortOrder = @sortOrder WHERE id = @id";
                SqlCommand updatePage = new SqlCommand(queryString, conn);
                updatePage.Parameters.AddWithValue("sortOrder", newSortOrder);
                updatePage.Parameters.AddWithValue("id", m_MenuItem.Id);
                updatePage.ExecuteNonQuery();

                SqlCommand updatePage2 = new SqlCommand(queryString, conn);
                updatePage2.Parameters.AddWithValue("sortOrder", oldSortOrder);
                updatePage2.Parameters.AddWithValue("id", o_MenuItem.Id);
                updatePage2.ExecuteNonQuery();
            }

            conn.Close();
        }

        public static void sortDown(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            MenuItem m_MenuItem = DBMenuItem.RetrieveOne(id);
            int oldSortOrder = m_MenuItem.SortOrder;
            int newSortOrder = oldSortOrder + 1;

            //check boundaries of sort to make sure they are valid

            string queryString = "SELECT TOP 1 id FROM CMS_MenuItems WHERE parentId = @parentId and sortOrder = @sortOrder ORDER BY id DESC";
            SqlCommand getId = new SqlCommand(queryString, conn);
            getId.Parameters.AddWithValue("parentId", m_MenuItem.ParentId);
            getId.Parameters.AddWithValue("sortOrder", newSortOrder);

            object myId = getId.ExecuteScalar();

            if (myId != null)
            {
                int m_id = Convert.ToInt32(myId);
                MenuItem o_MenuItem = DBMenuItem.RetrieveOne(m_id);

                queryString = "UPDATE CMS_MenuItems SET sortOrder = @sortOrder WHERE id = @id";
                SqlCommand updatePage = new SqlCommand(queryString, conn);
                updatePage.Parameters.AddWithValue("sortOrder", newSortOrder);
                updatePage.Parameters.AddWithValue("id", m_MenuItem.Id);
                updatePage.ExecuteNonQuery();

                SqlCommand updatePage2 = new SqlCommand(queryString, conn);
                updatePage2.Parameters.AddWithValue("sortOrder", oldSortOrder);
                updatePage2.Parameters.AddWithValue("id", o_MenuItem.Id);
                updatePage2.ExecuteNonQuery();
            }

            conn.Close();
        }
    }
}