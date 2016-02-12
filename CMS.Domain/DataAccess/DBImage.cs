using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;
using System.IO;
using System.Configuration;




namespace CMS.Domain.DataAccess
{
    public class DBImage
    {
        public static void Create(Image m_Image)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Images(name, altText, fileType, parentId, contentGroup) VALUES(@name, @altText, @fileType, @parentId, @contentGroup)";
            SqlCommand insertImage = new SqlCommand(queryString, conn);
            insertImage.Parameters.AddWithValue("name", m_Image.Name);
            insertImage.Parameters.AddWithValue("altText", m_Image.AltText);
            insertImage.Parameters.AddWithValue("fileType", m_Image.FileType ?? "");
            insertImage.Parameters.AddWithValue("parentId", m_Image.ParentId);
            insertImage.Parameters.AddWithValue("contentGroup", m_Image.ContentGroup);
            insertImage.ExecuteNonQuery();

            conn.Close();
        }
        public static Image RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Images WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getImage = new SqlCommand(queryString, conn);
            getImage.Parameters.AddWithValue("id", id);

            SqlDataReader imageDataReader = getImage.ExecuteReader();

            Image myImage = new Image();

            if (imageDataReader.Read())
            {
                myImage.Id = imageDataReader.GetInt32(0);
                myImage.Name = imageDataReader.GetString(1);
                myImage.AltText = imageDataReader.GetString(2);
                myImage.FileType = imageDataReader.GetString(3);
                myImage.ParentId = imageDataReader.GetInt32(4);
                myImage.ContentGroup = imageDataReader.GetInt32(5);
            }

            conn.Close();

            return myImage;
        }
        public static List<Image> RetrieveAll(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Images where parentId = @id AND pageWorkFlowState != 4 ORDER BY name";
            SqlCommand getImages = new SqlCommand(queryString, conn);
            getImages.Parameters.AddWithValue("id", id);

            List<Image> m_Images = new List<Image>();

            SqlDataReader allImages = getImages.ExecuteReader();

            while (allImages.Read())
            {
                Image m_Image = new Image();
                m_Image.Id = allImages.GetInt32(0);
                m_Image.Name = allImages.GetString(1);
                m_Image.AltText = allImages.GetString(2);
                m_Image.FileType = allImages.GetString(3);
                m_Image.ParentId = allImages.GetInt32(4);
                m_Image.ContentGroup = allImages.GetInt32(5);

                m_Images.Add(m_Image);
            }

            conn.Close();
            return m_Images;
        }

        public static void Update(Image m_Image)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Images SET name = @name, altText = @altText, fileType = @fileType, parentId = @parentId, contentGroup = @contentGroup WHERE id = @id";
            SqlCommand updateImage = new SqlCommand(queryString, conn);

            updateImage.Parameters.AddWithValue("name", m_Image.Name);
            updateImage.Parameters.AddWithValue("altText", m_Image.AltText);
            updateImage.Parameters.AddWithValue("fileType", m_Image.FileType);
            updateImage.Parameters.AddWithValue("parentId", m_Image.ParentId);
            updateImage.Parameters.AddWithValue("contentGroup", m_Image.ContentGroup);
            updateImage.Parameters.AddWithValue("id", m_Image.Id);

            updateImage.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            Image m_Image = DBImage.RetrieveOne(id);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Images SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand updateImage = new SqlCommand(queryString, conn);
            updateImage.Parameters.AddWithValue("id", id);
            updateImage.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_Images', @objectName, @deleteDate, @deletedBy, 'id', @objectType)";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_Image.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_Image.Name);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.Parameters.AddWithValue("objectType", m_Image.FileType);
            insertTrash.ExecuteNonQuery();

            conn.Close();
        }

        public static int GetParentId(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT parentId FROM CMS_Images WHERE id = @id";
            SqlCommand getImage = new SqlCommand(queryString, conn);
            getImage.Parameters.AddWithValue("id", id);
            int parentId = (int)getImage.ExecuteScalar();

            conn.Close();

            return parentId;
        }
    }
}