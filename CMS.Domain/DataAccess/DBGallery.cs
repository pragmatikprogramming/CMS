using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;

namespace CMS.Domain.DataAccess
{
    public class DBGallery
    {
        public static void Create(Gallery m_Gallery)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Gallery(name, contentGroup) VALUES(@name, @contentGroup)";
            SqlCommand insertGallery = new SqlCommand(queryString, conn);
            insertGallery.Parameters.AddWithValue("name", m_Gallery.Name);
            insertGallery.Parameters.AddWithValue("contentGroup", m_Gallery.ContentGroup);
            insertGallery.ExecuteNonQuery();

            conn.Close();
        }

        public static Gallery RetrieveOne(int id)
        {

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Gallery WHERE id = @id";
            SqlCommand getGallery = new SqlCommand(queryString, conn);
            getGallery.Parameters.AddWithValue("id", id);
            
            SqlDataReader m_Gallery = getGallery.ExecuteReader();

            Gallery myGallery = new Gallery();

            if(m_Gallery.Read())
            {
                myGallery.Id = m_Gallery.GetInt32(0);
                myGallery.Name = m_Gallery.GetString(1);
                myGallery.ContentGroup = m_Gallery.GetInt32(2);
            }

            conn.Close();

            return myGallery;
        }

        public static List<Gallery> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Gallery";
            SqlCommand getGalleries = new SqlCommand(queryString, conn);

            SqlDataReader m_Galleries = getGalleries.ExecuteReader();

            List<Gallery> myGalleries = new List<Gallery>();

            while(m_Galleries.Read())
            {
                Gallery tempGallery = new Gallery();

                tempGallery.Id = m_Galleries.GetInt32(0);
                tempGallery.Name = m_Galleries.GetString(1);
                tempGallery.ContentGroup = m_Galleries.GetInt32(2);

                myGalleries.Add(tempGallery);
            }

            conn.Close();

            return myGalleries;
            
        }

        public static void Update(Gallery m_Gallery)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Gallery SET name = @name, contentGroup = @contentGroup WHERE id = @id";
            SqlCommand updateGallery = new SqlCommand(queryString, conn);
            updateGallery.Parameters.AddWithValue("name", m_Gallery.Name);
            updateGallery.Parameters.AddWithValue("contentGroup", m_Gallery.ContentGroup);
            updateGallery.Parameters.AddWithValue("id", m_Gallery.Id);

            updateGallery.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM CMS_Gallery WHERE id = @id";
            SqlCommand deleteGallery = new SqlCommand(queryString, conn);
            deleteGallery.Parameters.AddWithValue("id", id);
            deleteGallery.ExecuteNonQuery();

            conn.Close();
        }
    }
}