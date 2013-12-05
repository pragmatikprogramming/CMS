using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;

namespace CMS.Domain.DataAccess
{
    public class DBMenu
    {
        public static void Create(Menu m_Menu)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Menus(menuName, pageWorkFlowState) VALUES(@menuName, 0)";
            SqlCommand insertMenu = new SqlCommand(queryString, conn);
            insertMenu.Parameters.AddWithValue("menuName", m_Menu.MenuName);
            insertMenu.ExecuteNonQuery();

            conn.Close();
        }

        public static Menu RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Menus WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getMenu = new SqlCommand(queryString, conn);
            getMenu.Parameters.AddWithValue("id", id);
            SqlDataReader menuDataReader = getMenu.ExecuteReader();

            Menu m_Menu = new Menu();

            if(menuDataReader.Read())
            {
                m_Menu.Id = menuDataReader.GetInt32(0);
                m_Menu.MenuName = menuDataReader.GetString(1);
                m_Menu.PageWorkFlowState = menuDataReader.GetInt32(2);
            }

            conn.Close();
            return m_Menu;
        }

        public static List<Menu> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Menus WHERE pageWorkFlowState != 4";
            SqlCommand getMenus = new SqlCommand(queryString, conn);
            SqlDataReader menuDataReader = getMenus.ExecuteReader();

            List<Menu> m_Menus = new List<Menu>();

            while (menuDataReader.Read())
            {
                Menu tempMenu = new Menu();

                tempMenu.Id = menuDataReader.GetInt32(0);
                tempMenu.MenuName = menuDataReader.GetString(1);
                tempMenu.PageWorkFlowState = menuDataReader.GetInt32(2);

                m_Menus.Add(tempMenu);
            }
            
            conn.Close();
            return m_Menus;

        }

        public static void Update(Menu m_Menu)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Menus SET menuName = @menuName WHERE id = @id";
            SqlCommand updateMenu = new SqlCommand(queryString, conn);
            updateMenu.Parameters.AddWithValue("menuName", m_Menu.MenuName);
            updateMenu.Parameters.AddWithValue("id", m_Menu.Id);
            updateMenu.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            Menu m_Menu = RetrieveOne(id);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Menus SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand updateMenu = new SqlCommand(queryString, conn);
            updateMenu.Parameters.AddWithValue("id", id);
            updateMenu.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_Menus', @objectName, @deleteDate, @deletedBy, 'id', 'Menu')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_Menu.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_Menu.MenuName);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();

            conn.Close();
        }

        public static void PurgeMenuItmes(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM CMS_MenuItems WHERE parentId = @id";
            SqlCommand delMenu = new SqlCommand(queryString, conn);
            delMenu.Parameters.AddWithValue("id", id);
            delMenu.ExecuteNonQuery();

            conn.Close();
        }
    }
}