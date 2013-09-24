using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IGalleryRepository
    {
        void CreateGallery(Gallery m_Gallery);
        Gallery RetrieveOne(int id);
        List<Gallery> RetrieveAll();
        void Update(Gallery m_Gallery, string OldName);
        bool DeleteGallery(int Id);
    }
}
