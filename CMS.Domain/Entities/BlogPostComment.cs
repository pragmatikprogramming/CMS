using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class BlogPostComment
    {
        private int id;
        private int blogId;
        private string name;
        private string comment;
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

        public int BlogId
        {
            get 
            { 
                return blogId; 
            }
            set 
            { 
                blogId = value; 
            }
        }

        [Required(ErrorMessage = "Please Enter a Name")]
        public string Name
        {
            get 
            { 
                return name; 
            }
            set 
            { 
                name = value; 
            }
        }

        [Required(ErrorMessage = "Please Enter a Comment")]
        public string Comment
        {
            get 
            { 
                return comment; 
            }
            set 
            { 
                comment = value; 
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