using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class MenuItemRepository : IMenuItemRepository
    {
        public void Create(MenuItem m_MenuItem)
        {
            DBMenuItem.Create(m_MenuItem);
        }

        public MenuItem RetrieveOne(int id)
        {
            MenuItem m_MenuItem = DBMenuItem.RetrieveOne(id);
            return m_MenuItem;
        }

        public List<MenuItem> RetrieveAll(int parentId)
        {
            List<MenuItem> m_MenuItems = DBMenuItem.RetrieveAll(parentId);
            return m_MenuItems;
        }

        public void Update(MenuItem m_MenuItem)
        {
            DBMenuItem.Update(m_MenuItem);
        }

        public void Delete(int id)
        {
            DBMenuItem.Delete(id);
        }

        public void sortUp(int id)
        {
            DBMenuItem.sortUp(id);
        }

        public void sortDown(int id)
        {
            DBMenuItem.sortDown(id);
        }
    }
}