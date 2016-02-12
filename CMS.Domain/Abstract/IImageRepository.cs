using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using System.Web;

namespace CMS.Domain.Abstract
{
    public interface IImageRepository
    {
        void Create(Image m_Image, HttpPostedFileBase myFile);
        Image RetrieveOne(int id);
        List<Image> RetrieveAll(int id);
        void Update(Image m_Image, HttpPostedFileBase fileUpload, string OldName, string OldFileType);
        void Delete(int id);
        byte[] ToBinary(HttpPostedFileBase myFile);
    }
}
