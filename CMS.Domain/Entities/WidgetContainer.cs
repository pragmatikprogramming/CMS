using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class WidgetContainer
    {
        private int id;
        private string name;
        private List<int> myWidgets;
        private List<HTMLWidget> widgets;

        
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

        [Required(ErrorMessage = "Please Enter Widgets to associate with the side bar")]
        public List<int> MyWidgets
        {
            get 
            { 
                return myWidgets; 
            }
            set 
            { 
                myWidgets = value; 
            }
        }

        public List<HTMLWidget> Widgets
        {
            get 
            { 
                return widgets; 
            }
            set 
            { 
                widgets = value; 
            }
        }
    }
}