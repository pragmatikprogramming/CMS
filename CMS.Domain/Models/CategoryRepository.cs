using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;
using CMS.Domain.HelperClasses;

namespace CMS.Domain.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        public void Create(Category m_Category)
        {
            DBCategory.Create(m_Category);
        }

        public Category RetrieveOne(int id)
        {
            Category m_Category = DBCategory.RetrieveOne(id);
            return m_Category;
        }

        public List<Category> RetrieveAll()
        {
            List<Category> m_Categories = DBCategory.RetrieveAll();
            return m_Categories;
        }

        public void Update(Category m_Category)
        {
            DBCategory.Update(m_Category);
        }

        public void Delete(int id)
        {
            DBCategory.Delete(id);
        }
    }
}