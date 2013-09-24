using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Image
    {
        private int id;
        private string name;
        private string fileType;
        private int contentGroup;
        private string altText;
        private int parentId;
        private int width;
        private int height;

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

        [Required(ErrorMessage = "Please select an Image Name")]
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

        [Required(ErrorMessage = "Please enter a value for Alternate Text")]
        public string AltText
        {
            get 
            { 
                return altText; 
            }
            set 
            { 
                altText = value; 
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

        public int Width
        {
            get 
            { 
                return width; 
            }
            set 
            { 
                width = value; 
            }
        }
        public int Height
        {
            get 
            { 
                return height; 
            }
            set 
            { 
                height = value; 
            }
        }
    }
}