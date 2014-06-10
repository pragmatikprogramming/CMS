using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Page
    {
        private int id;
        private int pageID;
        private int parentId;
        private int contentGroup;
        private int templateId;
        private string templateName;
        private string pageTitle;
        private string navigationName;
        private DateTime publishDate;
        private DateTime expireDate;
        private string content;
        private string metaDescription;
        private string metaKeywords;
        private int pageWorkFlowState;
        private int lockedBy;
        private string lockedByName;
        private string lastModifiedBy;
        private string lastModifiedDate;
        private int sortOrder;
        private string redirectURL;
        private int pageType;
        private int pageTypeId;
        private int displayOnSubmenu;
        private int bannerImage;
        private string bannerImageName;
        private string friendlyURL;
        private int widgetContainerId;
        private int displaySocialMedia;

        



        
        public int Id
        {
            get 
            { 
                return id; 
            }
            set 
            { 
                id = value; 
            }
        }

        public int PageID
        {
            get 
            { 
                return pageID; 
            }
            set
            {
                pageID = value;
            }
        }

        public int ParentId
        {
            get 
            { 
                return parentId; 
            }
            set 
            { 
                parentId = value; 
            }
        }

        [Required(ErrorMessage = "Please select a Content Group")]
        public int ContentGroup
        {
            get 
            { 
                return contentGroup; 
            }
            set 
            { 
                contentGroup = value; 
            }
        }

        [Required(ErrorMessage = "Please select a Template")]
        public int TemplateId
        {
            get 
            { 
                return templateId; 
            }
            set 
            { 
                templateId = value; 
            }
        }

        public string TemplateName
        {
            get 
            { 
                return templateName; 
            }
            set 
            { 
                templateName = value; 
            }
        }

        [Required(ErrorMessage = "Please Enter a Page Title")]
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

        [Required(ErrorMessage = "Please Enter a Navigation Name")]
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

        [Required(ErrorMessage = "Please select a Publish Date")]
        public DateTime PublishDate
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
        
        public DateTime ExpireDate
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

        public int PageWorkFlowState
        {
            get 
            { 
                return pageWorkFlowState; 
            }
            set 
            { 
                pageWorkFlowState = value; 
            }
        }

        public int LockedBy
        {
            get 
            { 
                return lockedBy; 
            }
            set 
            { 
                lockedBy = value; 
            }
        }

        public string LockedByName
        {
            get 
            { 
                return lockedByName; 
            }
            set 
            { 
                lockedByName = value; 
            }
        }

        public string LastModifiedBy
        {
            get 
            { 
                return lastModifiedBy; 
            }
            set 
            { 
                lastModifiedBy = value; 
            }
        }

        public string LastModifiedDate
        {
            get 
            { 
                return lastModifiedDate; 
            }
            set 
            { 
                lastModifiedDate = value; 
            }
        }

        public int SortOrder
        {
            get 
            { 
                return sortOrder; 
            }
            set 
            { 
                sortOrder = value; 
            }
        }

        public string RedirectURL
        {
            get 
            { 
                return redirectURL; 
            }
            set 
            { 
                redirectURL = value; 
            }
        }

        public int PageType
        {
            get 
            { 
                return pageType; 
            }
            set 
            { 
                pageType = value; 
            }
        }

        public int PageTypeId
        {
            get 
            { 
                return pageTypeId; 
            }
            set 
            { 
                pageTypeId = value; 
            }
        }

        public int DisplayOnSubmenu
        {
            get 
            { 
                return displayOnSubmenu; 
            }
            set 
            { 
                displayOnSubmenu = value; 
            }
        }

        public int BannerImage
        {
            get 
            { 
                return bannerImage; 
            }
            set 
            { 
                bannerImage = value; 
            }
        }

        public string BannerImageName
        {
            get 
            { 
                return bannerImageName; 
            }
            set 
            { 
                bannerImageName = value; 
            }
        }

        public string FriendlyURL
        {
            get
            {
                return friendlyURL;
            }
            set
            {
                friendlyURL = value;
            }
        }

        public int DisplaySocialMedia
        {
            get 
            { 
                return displaySocialMedia; 
            }
            set 
            { 
                displaySocialMedia = value; 
            }
        }

        public int WidgetContainerId
        {
            get 
            { 
                return widgetContainerId; 
            }
            set 
            { 
                widgetContainerId = value; 
            }
        }

        public Page()
        {
            content = string.Empty;
            metaDescription = string.Empty;
            metaKeywords = string.Empty;
            FriendlyURL = "";
        }

        public void PageSetDefaults()
        {
            if (RedirectURL == null)
            {
                RedirectURL = "";
            }
        }     
    }
}