using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;

namespace CMS.Domain.DataAccess
{
    public class DBPage
    {
        public void DBCreate(Page m_Page)
        {

        }

        public Page DBRetrieve(int m_ID)
        {
            object myObject = new object();
            return (Page)myObject;
        }

        public bool DBUpdate(Page m_Page)
        {
            return true;
        }

        public void DBDelete(int m_ID)
        {

        }
    }
}