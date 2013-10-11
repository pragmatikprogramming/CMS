using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface ICategoryRepository
    {
        void Create(Category m_Category);
        Category RetrieveOne(int id);
        List<Category> RetrieveAll();
        void Update(Category m_Category);
        void Delete(int id);
    }
}
