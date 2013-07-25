using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace CMS.Domain.HelperClasses
{
    public class DB
    {
        public DB()
        {

        }

        public static SqlConnection DbConnect()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["DBServer"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["DBName"] + ";User ID=" + ConfigurationManager.AppSettings["DBUser"] + ";Password=" + ConfigurationManager.AppSettings["DBPass"]);

            return conn;
        }
    }
}