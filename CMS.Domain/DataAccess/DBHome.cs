using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;
using CMS.Domain.Entities;

namespace CMS.Domain.DataAccess
{
    public class DBHome
    {
        public static List<Page> MainMenu()
        {
            List<Page> m_Pages = DBPage.RetrieveAll(0);
            return m_Pages;
        }
    }
}