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
            string thumbPath = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\thumbs\\" + m_Image.Name + "_thumb." + m_Image.FileType;
            myFile.SaveAs(path);

            using (System.Drawing.Image myImage = System.Drawing.Image.FromFile(path))
            {
                System.Drawing.Image thumb = myImage.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
                thumb.Save(thumbPath);
                ((IDisposable)myImage).Dispose();
                ((IDisposable)thumb).Dispose();
                GC.Collect();
            }

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

        public void Update(Image m_Image, HttpPostedFileBase fileUpload, string OldName, string OldFileType)
        {
            string fileExt;

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                fileExt = fileUpload.FileName.Split('.').Last();
                m_Image.FileType = fileExt;

                DBImage.Update(m_Image);

                Gallery m_Gallery = DBGallery.RetrieveOne(m_Image.ParentId);

                string path = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + OldName + "." + OldFileType;
                File.Delete(path);
                path = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + m_Image.Name + "." + m_Image.FileType;

                string thumbpath = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\thumbs\\" + OldName + "_thumb." + OldFileType;
                File.Delete(thumbpath);
                thumbpath = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\thumbs\\" + m_Image.Name + "_thumb." + m_Image.FileType;

                fileUpload.SaveAs(path);

                using (System.Drawing.Image myImage = System.Drawing.Image.FromFile(path))
                {
                    System.Drawing.Image thumb = myImage.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
                    thumb.Save(thumbpath);
                    ((IDisposable)myImage).Dispose();
                    ((IDisposable)thumb).Dispose();
                    GC.Collect();
                }
            }
            else if (OldName != m_Image.Name)
            {
                m_Image.FileType = OldFileType;
                DBImage.Update(m_Image);

                Gallery m_Gallery = DBGallery.RetrieveOne(m_Image.ParentId);

                string oldPath = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + OldName + "." + OldFileType;
                string newPath = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\" + m_Image.Name + "." + m_Image.FileType;

                string oldPathThumb = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\thumbs\\" + OldName + "_thumb." + OldFileType;
                string newPathThumb = ConfigurationManager.AppSettings["Gallery"] + "\\" + m_Gallery.Name + "\\thumbs\\" + m_Image.Name + "_thumb." + m_Image.FileType;

                File.Move(oldPath, newPath);
                File.Move(oldPathThumb, newPathThumb);
            }
            else
            {
                m_Image.FileType = OldFileType;

                DBImage.Update(m_Image);
            }
            
        }

        public void Delete(int id)
        {
            DBImage.Delete(id);
        }

        public byte[] ToBinary(HttpPostedFileBase myFile)
        {
            var content = new byte[0];

            if (myFile != null && myFile.ContentLength > 0)
            {
                content = new byte[myFile.ContentLength];
                myFile.InputStream.Read(content, 0, myFile.ContentLength);
            }

            return content;
        }
    }
}