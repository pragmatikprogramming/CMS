using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Folder
    {
        private int id; 
        private string name;
        private int parentId;

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

        [Required(ErrorMessage = "Please Name your folder")]
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

        [Required(ErrorMessage = "Something bad happened, some required data is missing. Please contact your administrator")]
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

    }
}