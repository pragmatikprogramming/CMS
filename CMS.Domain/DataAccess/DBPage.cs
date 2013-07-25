using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBPage
    {
        public void DBPageCreate(Page m_Page)
        {

        }

        public Page DBPageRetrieve(int m_ID)
        {
            object myObject = new object();
            return (Page)myObject;
        }

        public bool DBPageUpdate(Page m_Page)
        {
            return true;
        }

        public void DBPageDelete(int m_ID)
        {

        }
    }
}