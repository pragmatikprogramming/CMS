using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class MenuRepository : IMenuRepository
    {
        public void Create(Menu m_Menu)
        {
            DBMenu.Create(m_Menu);
        }

        public Menu RetrieveOne(int id)
        {
            Menu m_Menu = DBMenu.RetrieveOne(id);
            return m_Menu;
        }

        public List<Menu> RetrieveAll()
        {
            List<Menu> m_Menus = DBMenu.RetrieveAll();
            return m_Menus;
        }

        public void Update(Menu m_Menu)
        {
            DBMenu.Update(m_Menu);
        }

        public void Delete(int id)
        {
            DBMenu.Delete(id);
        }
    }
}