using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;


namespace CMS.Domain.DataAccess
{
    public class DBTrash
    {
        public static void Create(Trash m_Trash)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn) VALUES(@objectId, @objectTable, @objectName, @deleteDate, @deletedBy, @objectColumn)";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_Trash.ObjectId);
            insertTrash.Parameters.AddWithValue("objectTable", m_Trash.ObjectTable);
            insertTrash.Parameters.AddWithValue("objectName", m_Trash.ObjectName);
            insertTrash.Parameters.AddWithValue("deleteDate", m_Trash.DeleteDate);
            insertTrash.Parameters.AddWithValue("deletedBy", m_Trash.DeletedBy);
            insertTrash.Parameters.AddWithValue("objectColumn", m_Trash.ObjectColumn);

            insertTrash.ExecuteNonQuery();

            conn.Close();
        }

        public static Trash RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Trash WHERE id = @id";
            SqlCommand getTrash = new SqlCommand(queryString, conn);
            getTrash.Parameters.AddWithValue("id", id);
            SqlDataReader trashDataReader = getTrash.ExecuteReader();

            Trash m_Trash = new Trash();

            if(trashDataReader.Read())
            {
                m_Trash.Id = trashDataReader.GetInt32(0);
                m_Trash.ObjectId = trashDataReader.GetInt32(1);
                m_Trash.ObjectTable = trashDataReader.GetString(2);
                m_Trash.ObjectName = trashDataReader.GetString(3);
                m_Trash.DeleteDate = trashDataReader.GetDateTime(4);
                m_Trash.DeletedBy = trashDataReader.GetInt32(5);
                m_Trash.ObjectColumn = trashDataReader.GetString(6);
                m_Trash.ObjectType = trashDataReader.GetString(7);

                User m_User = DBUser.GetOne(m_Trash.DeletedBy);

                m_Trash.DeletedByName = m_User.FirstName + " " + m_User.LastName;
            }

            conn.Close();

            return m_Trash;
        }

        public static List<Trash> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Trash ORDER BY deleteDate DESC";
            SqlCommand getTrash = new SqlCommand(queryString, conn);
            SqlDataReader trashDataReader = getTrash.ExecuteReader();

            List<Trash> m_Trash = new List<Trash>();

            while (trashDataReader.Read())
            {
                Trash tempTrash = new Trash();

                tempTrash.Id = trashDataReader.GetInt32(0);
                tempTrash.ObjectId = trashDataReader.GetInt32(1);
                tempTrash.ObjectTable = trashDataReader.GetString(2);
                tempTrash.ObjectName = trashDataReader.GetString(3);
                tempTrash.DeleteDate = trashDataReader.GetDateTime(4);
                tempTrash.DeletedBy = trashDataReader.GetInt32(5);
                tempTrash.ObjectColumn = trashDataReader.GetString(6);
                tempTrash.ObjectType = trashDataReader.GetString(7);

                User m_User = DBUser.GetOne(tempTrash.DeletedBy);

                tempTrash.DeletedByName = m_User.FirstName + " " + m_User.LastName;

                m_Trash.Add(tempTrash);
            }

            conn.Close();

            return m_Trash;
        }

        public static void Restore(Trash m_Trash)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE " + m_Trash.ObjectTable + " SET pageWorkFlowState = 1 WHERE " + m_Trash.ObjectColumn + " = @objectId";
            SqlCommand updateObject = new SqlCommand(queryString, conn);
            updateObject.Parameters.AddWithValue("objectId", m_Trash.ObjectId);
            updateObject.ExecuteNonQuery();

            queryString = "DELETE FROM CMS_Trash WHERE id = @id";
            SqlCommand delTrash = new SqlCommand(queryString, conn);
            delTrash.Parameters.AddWithValue("id", m_Trash.Id);
            delTrash.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            Trash m_Trash = DBTrash.RetrieveOne(id);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM " + m_Trash.ObjectTable + " WHERE " + m_Trash.ObjectColumn+ " = @objectId";
            SqlCommand deleteObject = new SqlCommand(queryString, conn);
            deleteObject.Parameters.AddWithValue("objectTable", m_Trash.ObjectTable);
            deleteObject.Parameters.AddWithValue("objectColumn", m_Trash.ObjectColumn);
            deleteObject.Parameters.AddWithValue("objectId", m_Trash.ObjectId);
            deleteObject.ExecuteNonQuery();

            queryString = "DELETE FROM CMS_Trash WHERE id = @id";
            SqlCommand deleteTrash = new SqlCommand(queryString, conn);
            deleteTrash.Parameters.AddWithValue("id", id);
            deleteTrash.ExecuteNonQuery();

            conn.Close();

            if (m_Trash.ObjectTable == "CMS_Forms")
            {
                conn.Open();

                queryString = "DELETE FROM CMS_FormToFormFields WHERE formId = @formId";
                SqlCommand deleteForm = new SqlCommand(queryString, conn);
                deleteForm.Parameters.AddWithValue("formId", m_Trash.ObjectId);
                deleteForm.ExecuteNonQuery();

                conn.Close();
            }

            if (m_Trash.ObjectTable == "CMS_FormFields")
            {
                conn.Open();

                queryString = "DELETE FROM CMS_FormToFormFields WHERE formFieldId = @id";
                SqlCommand deleteFormField = new SqlCommand(queryString, conn);
                deleteFormField.Parameters.AddWithValue("id", m_Trash.ObjectId);
                deleteFormField.ExecuteNonQuery();

                queryString = "DELETE FROM CMS_FormFields WHERE parentId = @id";
                SqlCommand deleteChildren = new SqlCommand(queryString, conn);
                deleteChildren.Parameters.AddWithValue("id", m_Trash.ObjectId);
                deleteChildren.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}