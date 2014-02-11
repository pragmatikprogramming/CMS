using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;

namespace CMS.Domain.HelperClasses
{
    public class Logger
    {
        public static void LogEvent(string message)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Log(message) VALUES(@message)";
            SqlCommand insertEvent = new SqlCommand(queryString, conn);
            insertEvent.Parameters.AddWithValue("message", message);
            insertEvent.ExecuteNonQuery();

            conn.Close();
        }
    }
}