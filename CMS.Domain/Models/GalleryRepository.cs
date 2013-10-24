using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using System.IO;
using System.Configuration;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class GalleryRepository : IGalleryRepository
    {
        public void CreateGallery(Gallery m_Gallery)
        {
            DBGallery.Create(m_Gallery);
            Directory.CreateDirectory(ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name);
            Directory.CreateDirectory(ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\thumbs");
        }

        public Gallery RetrieveOne(int id)
        {
            Gallery m_Gallery = DBGallery.RetrieveOne(id);

            return m_Gallery;
        }

        public List<Gallery> RetrieveAll()
        {
            List<Gallery> m_Gallery = DBGallery.RetrieveAll();

            for (int i = 0; i < m_Gallery.Count; i++ )
            {
                List<string> m_Files = new List<string>();

                foreach (string file in Directory.GetFiles(ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery[i].Name))
                {
                    string fileName = file.Split('\\').Last();
                    m_Files.Add(fileName);
                }

                m_Gallery[i].Images = m_Files;
            }

            return m_Gallery;
        }

        public void Update(Gallery m_Gallery, string OldName)
        {
            DBGallery.Update(m_Gallery);

            if (m_Gallery.Name != OldName)
            {
                string source = ConfigurationManager.AppSettings["Gallery"] + "\\" + OldName;
                string destination = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name;
                Directory.Move(source, destination);
            }
        }

        public bool DeleteGallery(int Id)
        {
            Gallery m_Gallery = RetrieveOne(Id);
            string path = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name;
            int m_Count = Directory.GetFiles(path).Length;

            if (m_Count <= 0)
            {
                DBGallery.Delete(Id);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}