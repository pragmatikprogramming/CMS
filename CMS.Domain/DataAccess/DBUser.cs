using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;
using CMS.Domain.Entities;
using System.Configuration;

namespace CMS.Domain.DataAccess
{
    public class DBUser
    {
        public static List<User> GetAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT id, firstName, lastName, userName, email FROM CMS_Users";
            SqlCommand cmd = new SqlCommand(queryString, conn);

            SqlDataReader m_Users = cmd.ExecuteReader();

            List<User> myList = new List<User>();

            while (m_Users.Read())
            {
                User tempUser = new User();
                tempUser.Uid = m_Users.GetInt32(0);
                tempUser.FirstName = m_Users.GetString(1);
                tempUser.LastName = m_Users.GetString(2);
                tempUser.UserName = m_Users.GetString(3);
                tempUser.Email = m_Users.GetString(4);

                myList.Add(tempUser);
            }
            
            conn.Close();

            return myList;
        }

        public static SqlDataReader GetOne(int m_Uid)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT firstName, lastName, userName, email FROM CMS_Users";
            SqlCommand cmd = new SqlCommand(queryString, conn);

            SqlDataReader m_User = cmd.ExecuteReader();
            conn.Close();

            return m_User;
        }

        public static bool isUserNameAvailable(string userName)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT count(userName) FROM CMS_Users WHERE userName = @userName";
            SqlCommand userExist = new SqlCommand(queryString, conn);
            userExist.Parameters.AddWithValue("userName", userName);

            if ((int)userExist.ExecuteScalar() > 0)
            {
                conn.Close();
                return false;
            }
            else
            {
                conn.Close();
                return true;
            }
        }

        public static void userAdd(User m_User)
        {
            string passWord = BCrypt.HashPassword(m_User.PassWord, ConfigurationManager.AppSettings["Salt"]);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Users(userName, firstName, lastName, email, passWord) VALUES(@userName, @firstName, @lastName, @email, @passWord)";
            SqlCommand insertUser = new SqlCommand(queryString, conn);
            insertUser.Parameters.AddWithValue("userName", m_User.UserName);
            insertUser.Parameters.AddWithValue("firstName", m_User.FirstName);
            insertUser.Parameters.AddWithValue("lastName", m_User.LastName);
            insertUser.Parameters.AddWithValue("email", m_User.Email);
            insertUser.Parameters.AddWithValue("passWord", passWord);

            insertUser.ExecuteNonQuery();
        }

        public static void userDelete(int m_Uid)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM CMS_Users WHERE id = @uid";
            SqlCommand deleteUser = new SqlCommand(queryString, conn);
            deleteUser.Parameters.AddWithValue("uid", m_Uid);

            deleteUser.ExecuteNonQuery();
        }
    }
}