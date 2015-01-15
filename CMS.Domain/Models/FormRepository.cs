﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;
using System.Net.Mail;
using System.Configuration;

namespace CMS.Domain.Models
{
    public class FormRepository : IFormRepository
    {
        public int Create(Form m_Form)
        {
            int m_FormId = DBForm.Create(m_Form);
            return m_FormId;
        }
        
        public Form RetrieveOne(int id)
        {
            Form m_Form = DBForm.RetrieveOne(id);
            return m_Form;
        }

        public List<Form> RetrieveAll()
        {
            List<Form> m_Forms = DBForm.RetrieveAll();
            return m_Forms;
        }

        public void Update(Form m_Form)
        {
            m_Form.MyFormFields = PreserveSortOrder(m_Form.Id, m_Form.MyFormFields);
            DBForm.Update(m_Form);
        }

        public void Delete(int id)
        {
            DBForm.Delete(id);
        }

        public List<FormField> getFormFields(int id)
        {
            List<FormField> m_FormFields = DBForm.getFormFields(id);

            return m_FormFields;
        }

        public void SortUp(int parentId, int id)
        {
            DBForm.SortUp(parentId, id);
        }

        public void SortDown(int parentId, int id)
        {
            DBForm.SortDown(parentId, id);
        }

        public void ToggleRequired(int parentId, int id, int value)
        {
            DBForm.ToggleRequired(parentId, id, value);
        }

        public void InsertFormData(string formData, int formId)
        {
            DBForm.InsertFormData(formData, formId);
        }

        public void SendFormData(string to, string from, string body, string subject)
        {
            MailMessage mail = new MailMessage(from, to, subject, body);
            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = false;
            client.Send(mail);
        }

        public string SpecialExistsOnForm(int id)
        {
            string m_Label = DBForm.SpecialExistsOnForm(id);
            return m_Label;
        }

        public List<int> PreserveSortOrder(int formId, List<int> myFormFields)
        {
            List<FormField> m_FormFields = DBForm.getFormFields(formId);
            List<int> m_FormFieldsUpdated = new List<int>();

            foreach(FormField ff in m_FormFields)
            {
                if(myFormFields.Contains(ff.Id))
                {
                    m_FormFieldsUpdated.Add(ff.Id);
                    myFormFields.Remove(ff.Id);
                }
            }

            foreach(int ffid in myFormFields)
            {
                m_FormFieldsUpdated.Add(ffid);
            }

            return m_FormFieldsUpdated;
        }

        public string FormDataExtract(int FormId, string StartDate, string EndDate)
        {
            List<string> m_FormData = DBForm.FormDataExtract(FormId, StartDate, EndDate);

            string csv = "";
            string[] delimiters = new string[] { "^^" };
            string[] moreDelimiters = new string[] { "::" };
            string[] rowResult;
            string[] result;
            int count = 0;
            string header = "";
            string row = "";
            foreach (string formData in m_FormData)
            {
                rowResult = formData.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in rowResult)
                {
                    result = item.Split(moreDelimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (result.Length > 1)
                    {
                        if (count == 0)
                        {
                            header += (result[0].Replace(",", " ") + ",");
                            row += (result[1].Replace(",", " ") + ",");
                        }
                        else
                        {
                            row += (result[1].Replace(",", " ") + ",");
                        }
                    }
                    else if (result.Length == 1)
                    {
                        if (count == 0)
                        {
                            header += (result[0].Replace(",", " ") + ",");
                        }
                    }
                    else
                    {
                        header += ",";
                    }

                }

                count = 1;

                row = row.Replace(":", "");

                if (header.Length > 0)
                {
                    csv += header + "\n" + row + "\n";
                }
                else
                {
                    csv += row + "\n";
                }

                header = "";
                row = "";

            }

            return csv;

        }
    }
}