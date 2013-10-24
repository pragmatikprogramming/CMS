using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IMenuItemRepository
    {
        void Create(MenuItem m_MenuItem);
        MenuItem RetrieveOne(int id);
        List<MenuItem> RetrieveAll(int parentId);
        void Update(MenuItem m_MenuItem);
        void Delete(int id);
        void sortUp(int id);
        void sortDown(int id);
    }
}
