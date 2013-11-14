using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.DataAccess;
using CMS.Domain.Entities;
using System.IO;
using System.Configuration;

namespace CMS.Domain.Models
{
    public class DocumentRepository : IDocumentRepository
    {
        public void Create(Document m_Document, HttpPostedFileBase fileUpload)
        {
            string fileExt = fileUpload.FileName.Split('.').Last();
            m_Document.FileType = fileExt;

            DBDocument.Create(m_Document);

            string path = "";

            if (m_Document.ParentId != 0)
            {
                path = DBFolder.FolderPath(m_Document.ParentId);
                path += "\\" + m_Document.Name;
            }
            else
            {
                path = m_Document.Name;
            }

            fileUpload.SaveAs(ConfigurationManager.AppSettings["Documents"] + "\\" + path + "." + fileExt);
        }

        public Document RetrieveOne(int id)
        {
            Document m_Document = new Document();
            return m_Document;
        }

        public List<Document> RetrieveAll(int parentId)
        {
            List<Document> m_Documents = DBDocument.RetriveAll(parentId);
            return m_Documents;
        }

        public void Update(Document m_Document, string OldName, HttpPostedFileBase fileUpload)
        {
            DBDocument.Update(m_Document);
            string path = "";

            if (OldName != m_Document.Name)
            {
                if (m_Document.ParentId != 0)
                {
                    path = DBFolder.FolderPath(m_Document.ParentId);
                }

                string oldPath = ConfigurationManager.AppSettings["Documents"] + "\\" + path + "\\" + OldName + "." + m_Document.FileType;
                string newPath = ConfigurationManager.AppSettings["Documents"] + "\\" + path + "\\" + m_Document.Name + "." + m_Document.FileType;

                File.Move(oldPath, newPath);
            }
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                if (m_Document.ParentId != 0)
                {
                    path = DBFolder.FolderPath(m_Document.ParentId);
                    path += "\\" + m_Document.Name;
                }

                File.Delete(ConfigurationManager.AppSettings["Documents"] + "\\" + path);
                fileUpload.SaveAs(ConfigurationManager.AppSettings["Documents"] + "\\" + path);
            }
        }

        public bool Delete(int id)
        {
            string path = "";
            Document m_Document = DBDocument.RetrieveOne(id);
            DBDocument.Delete(id);

            /*if (m_Document.ParentId != 0)
            {
                path = DBFolder.FolderPath(m_Document.ParentId);
                path += "\\" + m_Document.Name;
            }
            else
            {
                path = m_Document.Name;
            }

            File.Delete(ConfigurationManager.AppSettings["Documents"] + "\\" + path + "." + m_Document.FileType);*/

            return true;
        }

        public void MoveDoc(int parentId, int id)
        {
            string oldPath = "";

            Document m_Document = DBDocument.RetrieveOne(id);

            if (m_Document.ParentId != 0)
            {
                oldPath = DBFolder.FolderPath(m_Document.ParentId);
                oldPath += "\\" + m_Document.Name + "." + m_Document.FileType;
            }
            else
            {
                oldPath = m_Document.Name + "." + m_Document.FileType;
            }

            string newPath = "";

            if (parentId != 0)
            {
                newPath = DBFolder.FolderPath(parentId);
                newPath += "\\" + m_Document.Name + "." + m_Document.FileType;
            }
            else
            {
                newPath = m_Document.Name + "." + m_Document.FileType;
            }

            DBDocument.MoveDoc(parentId, id);

            File.Move(ConfigurationManager.AppSettings["Documents"] + "\\" + oldPath, ConfigurationManager.AppSettings["Documents"] + "\\" + newPath);
        }
    }
}