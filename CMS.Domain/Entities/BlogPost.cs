using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class BlogPost
    {
        private int id;
        private int blogId;
        private string title;
        private DateTime publishDate;
        private int contentGroup;
        private List<int> categories;
        private string content;
        private int comments;
        private int pageWorkFlowState;
        private int lockedBy;
        private string lockedByName;
        private int lastModifiedBy;
        private string lastModifiedByName;
        private DateTime lastModifiedDate;

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

        [Required(ErrorMessage = "Please Enter a Title")]
        public string Title
        {
            get 
            { 
                return title; 
            }
            set 
            { 
                title = value; 
            }
        }

        [Required(ErrorMessage = "Please Enter a Publish Date")]
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

        [Required(ErrorMessage = "Please Select a Content Group")]
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

        [Required(ErrorMessage = "At least one Category must be selected")]
        public List<int> Categories
        {
            get
            { 
                return categories; 
            }
            set 
            { 
                categories = value; 
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

        public int Comments
        {
            get 
            { 
                return comments; 
            }
            set 
            { 
                comments = value; 
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

        public int LastModifiedBy
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

        public string LastModifiedByName
        {
            get 
            { 
                return lastModifiedByName; 
            }
            set 
            { 
                lastModifiedByName = value; 
            }
        }

        public DateTime LastModifiedDate
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
    }
}