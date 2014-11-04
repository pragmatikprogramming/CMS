using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBSystemSettings
    {
        public static SystemSettings GetSystemSettings()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_SystemSettings";
            SqlCommand getSettings = new SqlCommand(queryString, conn);
            SqlDataReader settingsReader = getSettings.ExecuteReader();

            SystemSettings m_Settings = new SystemSettings();

            if (settingsReader.Read())
            {
                m_Settings.DomainName = settingsReader.GetString(0);
                m_Settings.ImageBinary = (byte[])settingsReader[1];
            }


            conn.Close();

            return m_Settings;
        }

        public static void UpdateSystemSettings(SystemSettings m_Settings)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_SystemSettings SET domainName = @domainName, imageBinary = @imageBinary";
            SqlCommand updSettings = new SqlCommand(queryString, conn);
            updSettings.ExecuteNonQuery();
        }
    }
}