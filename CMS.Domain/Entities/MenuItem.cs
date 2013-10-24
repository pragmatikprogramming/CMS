using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class MenuItem
    {
        private int id;
        private int parentId;
        private string menuItemName;
        private string linkUrl;
        private int pageWorkFlowState;
        private int sortOrder;

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

        [Required(ErrorMessage = "Please name your Menu Item")]
        public string MenuItemName
        {
            get 
            { 
                return menuItemName; 
            }
            set 
            { 
                menuItemName = value; 
            }
        }

        [Required(ErrorMessage = "Please enter a Link Url")]
        public string LinkUrl
        {
            get 
            { 
                return linkUrl; 
            }
            set 
            { 
                linkUrl = value; 
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
    }
}