using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBCategory
    {
        public static void Create(Category m_Category)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Categories(categoryName, pageWorkFlowState) VALUES(@categoryName, 1)";
            SqlCommand insertCategory = new SqlCommand(queryString, conn);
            insertCategory.Parameters.AddWithValue("categoryName", m_Category.CategoryTitle);
            insertCategory.ExecuteNonQuery();

            conn.Close();
        }

        public static Category RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Categories WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getCategory = new SqlCommand(queryString, conn);
            getCategory.Parameters.AddWithValue("id", id);
            SqlDataReader categoryReader = getCategory.ExecuteReader();

            Category m_Category = new Category();

            if (categoryReader.Read())
            {
                m_Category.Id = categoryReader.GetInt32(0);
                m_Category.CategoryTitle = categoryReader.GetString(1);
                m_Category.PageWorkFlowState = categoryReader.GetInt32(2);
            }

            conn.Close();
            return m_Category;
        }

        public static List<Category> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Categories WHERE pageWorkFlowState != 4";
            SqlCommand getCategories = new SqlCommand(queryString, conn);
            SqlDataReader categoryReader = getCategories.ExecuteReader();

            List<Category> m_Categories = new List<Category>();

            while (categoryReader.Read())
            {
                Category tempCategory = new Category();

                tempCategory.Id = categoryReader.GetInt32(0);
                tempCategory.CategoryTitle = categoryReader.GetString(1);
                tempCategory.PageWorkFlowState = categoryReader.GetInt32(2);

                m_Categories.Add(tempCategory);
            }

            conn.Close();
            return m_Categories;
        }

        public static void Update(Category m_Category)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Categories SET categoryName = @categoryName WHERE id = @id";
            SqlCommand updateCategory = new SqlCommand(queryString, conn);
            updateCategory.Parameters.AddWithValue("categoryName", m_Category.CategoryTitle);
            updateCategory.Parameters.AddWithValue("id", m_Category.Id);
            updateCategory.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            Category m_Category = DBCategory.RetrieveOne(id);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_Categories', @objectName, @deleteDate, @deletedBy, 'id', @objectType)";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_Category.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_Category.CategoryTitle);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.Parameters.AddWithValue("objectType", "Category");
            insertTrash.ExecuteNonQuery();

            queryString = "UPDATE CMS_Categories SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand updateCategory = new SqlCommand(queryString, conn);
            updateCategory.Parameters.AddWithValue("id", id);
            updateCategory.ExecuteNonQuery();

            conn.Close();
        }
    }
}