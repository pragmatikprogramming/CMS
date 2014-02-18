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
    public class TrashRepository : ITrashRepository
    {
        public void Create(Trash m_Trash)
        {

        }

        public Trash RetrieveOne(int id)
        {
            Trash m_Trash = new Trash();
            return m_Trash;
        }

        public List<Trash> RetrieveAll()
        {
            List<Trash> m_Trash = DBTrash.RetrieveAll();
            return m_Trash;
        }

        public void Restore(int id)
        {
            Trash m_Trash = DBTrash.RetrieveOne(id);
            DBTrash.Restore(m_Trash);
        }

        public void Delete(int id)
        { 
            Trash m_Trash = DBTrash.RetrieveOne(id);

            if (m_Trash.ObjectTable == "CMS_Gallery")
            {
                string path = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Trash.ObjectName;
                Directory.Delete(path, true);
            }

            if (m_Trash.ObjectTable == "CMS_Images")
            {
                Gallery m_Gallery = DBGallery.RetrieveOne(DBImage.GetParentId(m_Trash.ObjectId));
                string path = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + m_Trash.ObjectName + "." + m_Trash.ObjectType;
                File.Delete(path);

                string thumbpath = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\thumbs\\" + m_Trash.ObjectName + "_thumb." + m_Trash.ObjectType;
                File.Delete(thumbpath);
            }

            if (m_Trash.ObjectTable == "CMS_Folders")
            {
                string path;

                if (DBFolder.GetParentId(m_Trash.ObjectId) != 0)
                {
                    path = DBFolder.FolderPath(m_Trash.ObjectId);
                }
                else
                {
                    path = m_Trash.ObjectName;
                }

                Directory.Delete(ConfigurationManager.AppSettings["Documents"] + "\\" + path);
            }

            if (m_Trash.ObjectTable == "CMS_Documents")
            {
                string path = "";

                if (DBDocument.GetParentId(m_Trash.ObjectId) != 0)
                {
                    path = DBFolder.FolderPath(DBDocument.GetParentId(m_Trash.ObjectId));
                    path += "\\" + m_Trash.ObjectName + "." + m_Trash.ObjectType;
                }
                else
                {
                    path = m_Trash.ObjectName;
                }

                File.Delete(ConfigurationManager.AppSettings["Documents"] + "\\" + path);
            }

            if (m_Trash.ObjectTable == "CMS_Menus")
            {
                DBMenu.PurgeMenuItmes(m_Trash.ObjectId);
            }

            DBTrash.Delete(id);
        }

    }
}