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
    }
}