using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.HelperClasses
{
    public class SessionHandler
    {
        public static void add_user_session(int uid)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_User_Session(uid, usersLastRequest) values(@uid, GETDATE())";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void add_failed_attempt(string userName)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT id FROM CMS_Users WHERE userName = @userName";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.AddWithValue("userName", userName);

            SqlDataReader userRecord = cmd.ExecuteReader();

            if (userRecord.Read())
            {
                int uid = userRecord.GetInt32(0);
                conn.Close();
                conn.Open();
                queryString = "INSERT INTO CMS_User_Failed_Attempts(uid, failedAttemptDateTime) values(@uid, GETDATE())";
                cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("uid", uid);

                cmd.ExecuteNonQuery();
                
            }

            conn.Close();
        }

        public static void delete_old_failed_attempt()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM CMS_User_Failed_Attempts WHERE failedAttemptDateTime < DATEADD(MINUTE, -30, GETDATE())";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static bool is_user_authenticated(int uid)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT COUNT(*) FROM CMS_User_Session WHERE uid = @uid AND usersLastRequest > DATEADD(MINUTE, -60, GETDATE())";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.AddWithValue("uid", uid);

            int num_records = (int)cmd.ExecuteScalar();

            conn.Close();

            if (num_records > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool is_user_locked(int uid)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT COUNT(*) FROM CMS_User_Failed_Attempts WHERE failedAttemptDateTime > DATEADD(MINUTE, -30, GETDATE()) AND uid = @uid";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.AddWithValue("uid", uid);
            int num_attempts = (int)cmd.ExecuteScalar();

            conn.Close();

            if (num_attempts >= 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool authenticate(string userName, string passWord)
        {
            //clean up previous session history
            delete_old_failed_attempt();

            passWord = BCrypt.HashPassword(passWord, ConfigurationManager.AppSettings["Salt"]);

            SqlConnection conn = DB.DbConnect();
            conn.Open();
            
            string queryString = "SELECT id, firstName, lastName FROM CMS_Users where userName = @userName and passWord = @passWord";
            SqlCommand cmd = new SqlCommand(queryString, conn);

            cmd.Parameters.AddWithValue("userName", userName);
            cmd.Parameters.AddWithValue("passWord", passWord);

            SqlDataReader userRecord = cmd.ExecuteReader();

            if (userRecord.Read())
            {
                SystemSettings m_Settings = new SystemSettings();
                m_Settings = DBSystemSettings.GetSystemSettings();

                HttpContext.Current.Session["uid"] = userRecord.GetInt32(0);
                HttpContext.Current.Session["Name"] = userRecord.GetString(1) + " " + userRecord.GetString(2);
                HttpContext.Current.Session["BarColor"] = m_Settings.BarColor;

                if (is_user_locked((int)HttpContext.Current.Session["uid"]))
                {
                    return false;
                }

                conn.Close();

                add_user_session((int)HttpContext.Current.Session["uid"]);

                return true;
            }
            else
            {
                conn.Close();
                add_failed_attempt(userName);
                return false;
            }          
        }
    }
}