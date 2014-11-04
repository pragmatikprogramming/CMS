using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Domain.Entities
{
    public class SystemSettings
    {
        string domainName;
        byte[] imageBinary;


        public string DomainName
        {
            get
            {
                return domainName;
            }
            set
            {
                domainName = value;
            }
        }

        public byte[] ImageBinary
        {
            get
            {
                return imageBinary;
            }
            set
            {
                imageBinary = value;
            }
        }
    }
}