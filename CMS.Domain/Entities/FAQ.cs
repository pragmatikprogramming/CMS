using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class FAQ
    {
        private int faqID;  
        private string faqName;
        private int contentGroup;
        private int pageWorkFlowState;

        public int FaqID
        {
            get 
            { 
                return faqID; 
            }
            set 
            { 
                faqID = value; 
            }
        }

        [Required(ErrorMessage = "Please name your FAQ list")]
        public string FaqName
        {
            get 
            { 
                return faqName; 
            }
            set 
            { faqName = value; 
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
    }
}