using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.Abstract;
using CMS.Domain.DataAccess;
using System.IO;
using System.Configuration;

namespace CMS.Domain.Models
{
    public class FolderRepository : IFolderRepository
    {
        public void Create(Folder m_Folder)
        {
            string path;
            DBFolder.Create(m_Folder);

            if (m_Folder.ParentId != 0)
            {
                path = DBFolder.FolderPath(m_Folder.ParentId);
                path += "\\" + m_Folder.Name;
            }
            else
            {
                path = m_Folder.Name;
            }

            Directory.CreateDirectory(ConfigurationManager.AppSettings["Documents"] + "\\" + path);
        }

        public Folder RetrieveOne(int id)
        {
            Folder m_Folder = DBFolder.RetrieveOne(id);
            return m_Folder;
        }

        public List<Folder> RetrieveAll(int id)
        {
            List<Folder> m_Folders = DBFolder.RetrieveAll(id);
            return m_Folders;
        }

        public void Update(Folder m_Folder, string OldName)
        {
            DBFolder.Update(m_Folder);

            if (m_Folder.Name != OldName)
            {
                string path, OldPath;

                if (m_Folder.ParentId != 0)
                {
                    path = DBFolder.FolderPath(m_Folder.ParentId);
                    OldPath = path;
                    OldPath += "\\" + OldName;
                    path += "\\" + m_Folder.Name;
                }
                else
                {
                    path = m_Folder.Name;
                    OldPath = OldName;
                }


                string source = ConfigurationManager.AppSettings["Documents"] + "\\" + OldPath;
                string destination = ConfigurationManager.AppSettings["Documents"] + "\\" + path;
                Directory.Move(source, destination);
            }
        }

        public void Delete(int id)
        {
            //Folder m_Folder = DBFolder.RetrieveOne(id);

            if (DBFolder.FolderCheckChildren(id))
            {
                DBFolder.Delete(id);
            }

            /*string path;

            if (m_Folder.ParentId != 0)
            {
                path = DBFolder.FolderPath(m_Folder.ParentId);
                path += "\\" + m_Folder.Name;
            }
            else
            {
                path = m_Folder.Name;
            }*/

            //Directory.Delete(ConfigurationManager.AppSettings["Documents"] + "\\" + path);
        }
    }
}