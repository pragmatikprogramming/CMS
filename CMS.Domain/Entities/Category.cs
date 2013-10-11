using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Category
    {
        private int id;
        private string categoryTitle;
        private int pageWorkFlowState;


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

        [Required(ErrorMessage = "Please enter a valid name for your category")]
        public string CategoryTitle
        {
            get 
            { 
                return categoryTitle; 
            }
            set 
            { 
                categoryTitle = value; 
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