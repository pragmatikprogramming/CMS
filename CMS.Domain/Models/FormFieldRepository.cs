using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class FormFieldRepository : IFormFieldRepository
    {
        public void Create(FormField m_FormField, string[] childrenTitle)
        {
            if (childrenTitle != null)
            {
                foreach (string c_Title in childrenTitle)
                {
                    if (c_Title.Length > 0)
                    {
                        FormField temp = new FormField();
                        temp.Label = c_Title;
                        temp.FieldType = m_FormField.FieldType;

                        m_FormField.Children.Add(temp);
                    }
                }
            }

            DBFormField.Create(m_FormField);
        }

        public FormField RetrieveOne(int id)
        {
            FormField m_FormField = DBFormField.RetrieveOne(id);
            return m_FormField;
        }

        public List<FormField> RetrieveAll()
        {
            List<FormField> m_FormFields = DBFormField.RetrieveAll();
            return m_FormFields;
        }

        public List<FormField> RetrieveChildren(int parentId)
        {
            List<FormField> m_FormFields = DBFormField.RetrieveChildren(parentId);
            return m_FormFields;
        }

        public void Update(FormField m_FormField, string[] childrenTitle)
        {
            DBFormField.DeleteChildren(m_FormField.Id);

            foreach (string label in childrenTitle)
            {
                if (label.Length > 0)
                {
                    FormField temp = new FormField();
                    temp.Label = label;
                    temp.FieldType = m_FormField.FieldType;
                    temp.ParentId = m_FormField.Id;

                    DBFormField.Create(temp);
                }
            }

            DBFormField.Update(m_FormField);
        }

        public void Delete(int id)
        {
            DBFormField.Delete(id);
        }

        public Dictionary<int, string> getFieldTypes()
        {
            Dictionary<int, string> m_FieldTypes = DBFormField.getFieldTypes();
            return m_FieldTypes;
        }

        public Dictionary<int, string> getValidationTypes()
        {
            Dictionary<int, string> m_Types = DBFormField.getValidationTypes();
            return m_Types;
        }
    }
}