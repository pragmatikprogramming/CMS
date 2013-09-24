using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Domain.Entities
{
    public class ContentGroup
    {
        private int groupID;
        private string contentGroupName;

        public string ContentGroupName
        {
            get 
            { 
                return contentGroupName; 
            }
            set 
            { 
                contentGroupName = value; 
            }
        }

        public int GroupID
        {
            get 
            { 
                return groupID; 
            }
            set 
            { 
                groupID = value; 
            }
        }
    }
}