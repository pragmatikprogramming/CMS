using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IFormFieldRepository
    {
        void Create(FormField m_FormField, string[] childrenTitle, string[] childrenValue);
        FormField RetrieveOne(int id);
        List<FormField> RetrieveAll();
        List<FormField> RetrieveChildren(int parentId);
        void Update(FormField m_FormField, string[] childrenTitle, string[] childrenValue);
        void Delete(int id);
        Dictionary<int, string> getFieldTypes();
        Dictionary<int, string> getValidationTypes();
    }
}
