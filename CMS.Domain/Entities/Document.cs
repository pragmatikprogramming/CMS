using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Document
    {
        private int id;    
        private string name;      
        private int parentId;     
        private string fileType;
        private int contentGroup;

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

        [Required(ErrorMessage = "Please enter a valid name for the document")]
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

        public string FileType
        {
            get 
            { 
                return fileType; 
            }
            set 
            { 
                fileType = value; 
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
    }
}