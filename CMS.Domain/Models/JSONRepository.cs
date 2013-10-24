using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class JSONRepository : IJSONRepository
    {
        public List<JSONImages> getImages()
        {
            List<JSONImages> m_Images = DBJSON.getImages();
            return m_Images;
        }
    }
}