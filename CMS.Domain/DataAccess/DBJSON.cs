using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;

namespace CMS.Domain.DataAccess
{
    public class DBJSON
    {
        public static List<JSONImages> getImages()
        {
            List<Gallery> m_Gallerys = DBGallery.RetrieveAll();
            List<JSONImages> m_JSONImages = new List<JSONImages>();

            foreach (Gallery m_Gallery in m_Gallerys)
            {
                List<Image> m_Images = DBImage.RetrieveAll(m_Gallery.Id);
                foreach (Image m_Image in m_Images)
                {
                    JSONImages tempImage = new JSONImages();
                    tempImage.thumb = "/Galleries/" + m_Gallery.Name + "/thumbs/" + m_Image.Name + "_thumb." + m_Image.FileType;
                    tempImage.image = "/Galleries/" + m_Gallery.Name + "/" + m_Image.Name + "." + m_Image.FileType;
                    tempImage.folder = m_Gallery.Name;

                    m_JSONImages.Add(tempImage);
                }
            }

            return m_JSONImages;
        }
    }
}