using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IMenuRepository
    {
        void Create(Menu m_Menu);
        Menu RetrieveOne(int id);
        List<Menu> RetrieveAll();
        void Update(Menu m_Menu);
        void Delete(int id);
    }
}
