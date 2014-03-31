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
        public void Create(FormField m_FormField, string[] childrenTitle, string[] childrenValue)
        {
            if (childrenTitle != null)
            {
                int count = 0;

                foreach (string c_Title in childrenTitle)
                {
                    if (m_FormField.FieldType == 10)
                    {
                        if (c_Title.Length > 0 && childrenValue[count].Length > 0)
                        {
                            FormField temp = new FormField();
                            temp.Label = c_Title + ":" + childrenValue[count];
                            temp.FieldType = m_FormField.FieldType;

                            m_FormField.Children.Add(temp);
                        }
                    }
                    else
                    {
                        if (c_Title.Length > 0)
                        {
                            FormField temp = new FormField();
                            temp.Label = c_Title;
                            temp.FieldType = m_FormField.FieldType;

                            m_FormField.Children.Add(temp);
                        }
                    }
                    count++;
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

        public void Update(FormField m_FormField, string[] childrenTitle, string[] childrenValue)
        {
            DBFormField.DeleteChildren(m_FormField.Id);

            if (childrenTitle != null)
            {
                int count = 0;

                foreach (string label in childrenTitle)
                {
                    if (m_FormField.FieldType == 10)
                    {
                        if (label.Length > 0 && childrenValue[count].Length > 0)
                        {
                            FormField temp = new FormField();
                            temp.Label = label + ":" + childrenValue[count];
                            temp.FieldType = m_FormField.FieldType;
                            temp.ParentId = m_FormField.Id;

                            DBFormField.Create(temp);
                        }
                    }
                    else
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
                    count++;
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