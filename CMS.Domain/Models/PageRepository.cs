using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;


namespace CMS.Domain.Models
{
    public class PageRepository : IPageRepository
    {
        public void Create(Page m_Page)
        {

        }

        public Page Retrieve(int m_ID)
        {
            object myObject = new object();
            return (Page)myObject;
        }

        public bool Update(Page m_Page)
        {
            return true;
        }

        public void Delete(int m_ID)
        {

        }
    }
}