using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;
using System.Configuration;
using System.IO;

namespace CMS.Domain.Models
{
    public class ImageRepository : IImageRepository
    {
        public void Create(Image m_Image, HttpPostedFileBase myFile)
        {

            string fileExt = myFile.FileName.Split('.').Last();
            m_Image.FileType = fileExt;

            DBImage.Create(m_Image);

            Gallery m_Gallery = DBGallery.RetrieveOne(m_Image.ParentId);
            string path = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + m_Image.Name + "." + m_Image.FileType;
            myFile.SaveAs(path);

        }

        public Image RetrieveOne(int id)
        {
            Image m_Image = DBImage.RetrieveOne(id);
            return m_Image;
        }

        public List<Image> RetrieveAll(int id)
        {
            List<Image> m_Images = DBImage.RetrieveAll(id);

            return m_Images;
        }

        public void Update(Image m_Image, HttpPostedFileBase fileUpload, string OldName)
        {
            DBImage.Update(m_Image);
            
            if (OldName != m_Image.Name)
            {
                Gallery m_Gallery = DBGallery.RetrieveOne(m_Image.ParentId);

                string oldPath = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + OldName + "." + m_Image.FileType;
                string newPath = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + m_Image.Name + "." + m_Image.FileType;

                File.Move(oldPath, newPath);
            }
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                Gallery m_Gallery = DBGallery.RetrieveOne(m_Image.ParentId);

                string path = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + m_Image.Name + "." + m_Image.FileType;
                File.Delete(path);

                fileUpload.SaveAs(path);
            }
        }

        public void Delete(int id)
        {

        }
    }
}