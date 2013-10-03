using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBFolder
    {
        public static void Create(Folder m_Folder)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Folders(name, parentId) VALUES(@name, @parentId)";
            SqlCommand insertFolder = new SqlCommand(queryString, conn);
            insertFolder.Parameters.AddWithValue("name", m_Folder.Name);
            insertFolder.Parameters.AddWithValue("parentId", m_Folder.ParentId);

            insertFolder.ExecuteNonQuery();

            conn.Close();
        }

        public static Folder RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Folders WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getFolder = new SqlCommand(queryString, conn);
            getFolder.Parameters.AddWithValue("id", id);
            SqlDataReader folderDataReader = getFolder.ExecuteReader();

            Folder m_Folder = new Folder();

            if (folderDataReader.Read())
            {
                m_Folder.Id = folderDataReader.GetInt32(0);
                m_Folder.Name = folderDataReader.GetString(1);
                m_Folder.ParentId = folderDataReader.GetInt32(2);
            }

            conn.Close();
            return m_Folder;
        }

        public static List<Folder> RetrieveAll(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Folders WHERE parentId = @parentId AND pageWorkFlowState != 4";
            SqlCommand getFolders = new SqlCommand(queryString, conn);
            getFolders.Parameters.AddWithValue("parentId", id);

            SqlDataReader foldersDataReader = getFolders.ExecuteReader();

            List<Folder> m_Folders = new List<Folder>();

            while (foldersDataReader.Read())
            {
                Folder tempFolder = new Folder();
                tempFolder.Id = foldersDataReader.GetInt32(0);
                tempFolder.Name = foldersDataReader.GetString(1);
                tempFolder.ParentId = foldersDataReader.GetInt32(2);
                m_Folders.Add(tempFolder);
            }
            
            conn.Close();

            return m_Folders;
        }

        public static void Update(Folder m_Folder)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Folders SET name = @name, parentId = @parentId WHERE id = @id";
            SqlCommand updateFolder = new SqlCommand(queryString, conn);
            updateFolder.Parameters.AddWithValue("name", m_Folder.Name);
            updateFolder.Parameters.AddWithValue("parentId", m_Folder.ParentId);
            updateFolder.Parameters.AddWithValue("id", m_Folder.Id);
            updateFolder.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            Folder m_Folder = DBFolder.RetrieveOne(id);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_Folders', @objectName, @deleteDate, @deletedBy, 'id', 'Folder')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_Folder.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_Folder.Name);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();

            queryString = "UPDATE CMS_Folders SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand deleteFolder = new SqlCommand(queryString, conn);
            deleteFolder.Parameters.AddWithValue("id", id);
            deleteFolder.ExecuteNonQuery();

            conn.Close();
        }

        public static int GetParentId(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT parentId FROM CMS_Folders WHERE id = @id";
            SqlCommand getParentId = new SqlCommand(queryString, conn);
            getParentId.Parameters.AddWithValue("id", id);
            int m_ParentId = (int)getParentId.ExecuteScalar();

            conn.Close();

            return m_ParentId;
        }

        public static string FolderPath(int parentId)
        {
            string path = "";

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT parentId, name from CMS_Folders WHERE id = @parentid";
            SqlCommand getFolder = new SqlCommand(queryString, conn);
            getFolder.Parameters.AddWithValue("parentId", parentId);
            SqlDataReader folderDataReader = getFolder.ExecuteReader();

            if (folderDataReader.Read())
            {
                if (folderDataReader.GetInt32(0) == 0)
                {
                    path = folderDataReader.GetString(1);
                    conn.Close();
                    return path;
                }
                else
                {
                    path = "\\" + DBFolder.FolderPath(folderDataReader.GetInt32(0));
                    path += "\\" + folderDataReader.GetString(1);
                    conn.Close();
                    return path;
                }
            }

            conn.Close();

            return path;
        }

        public static bool FolderCheckChildren(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Folders WHERE parentId = @parentId";
            SqlCommand getFolders = new SqlCommand(queryString, conn);
            getFolders.Parameters.AddWithValue("parentId", id);
            SqlDataReader foldersDataReader = getFolders.ExecuteReader();

            if (foldersDataReader.Read())
            {
                conn.Close();
                return false;
            }

            conn.Close();
            conn.Open();

            queryString = "SELECT * FROM CMS_Documents WHERE parentId = @parentId";
            SqlCommand getDocuments = new SqlCommand(queryString, conn);
            getDocuments.Parameters.AddWithValue("parentId", id);
            foldersDataReader = getDocuments.ExecuteReader();

            if (foldersDataReader.Read())
            {
                conn.Close();
                return false;
            }

            conn.Close();

            return true;

        }
    }
}