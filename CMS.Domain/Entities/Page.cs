using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Domain.Entities
{
    public class Page
    {
        private int pageID;
        private string pageTitle;
        private string navigationName;
        private string publishDate;
        private string expireDate;
        private string content;
        private string reviewSchedule;
        private string metaDescription;
        private string metaKeywords;



        public int PageID
        {
            get 
            { 
                return pageID; 
            }
        }
        public string PageTitle
        {
            get 
            { 
                return pageTitle; 
            }
            set 
            { 
                pageTitle = value; 
            }
        }
        public string NavigationName
        {
            get 
            { 
                return navigationName; 
            }
            set 
            { 
                navigationName = value; 
            }
        }
        public string PublishDate
        {
            get 
            { 
                return publishDate; 
            }
            set 
            { 
                publishDate = value; 
            }
        }
        public string ExpireDate
        {
            get 
            { 
                return expireDate; 
            }
            set 
            { 
                expireDate = value; 
            }
        }
        public string Content
        {
            get 
            { 
                return content; 
            }
            set 
            { 
                content = value; 
            }
        }
        public string ReviewSchedule
        {
            get 
            { 
                return reviewSchedule; 
            }
            set 
            { 
                reviewSchedule = value; 
            }
        }
        public string MetaDescription
        {
            get 
            { 
                return metaDescription; 
            }
            set 
            { 
                metaDescription = value; 
            }
        }
        public string MetaKeywords
        {
            get 
            { 
                return metaKeywords; 
            }
            set 
            { 
                metaKeywords = value; 
            }
        }
    }
}