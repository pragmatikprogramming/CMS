using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class FormField
    {
        private int id;
        private string label;
        private string labelText;
        private int fieldType;
        private string fieldTypeText;
        private int validationType;
        private int parentId;
        private int isRequired;
        private List<FormField> children;

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

        [Required(ErrorMessage = "Please provide a label")]
        public string Label
        {
            get 
            { 
                return label; 
            }
            set 
            { 
                label = value; 
            }
        }

        public string LabelText
        {
            get 
            { 
                return labelText; 
            }
            set 
            { 
                labelText = value; 
            }
        }

        [Required(ErrorMessage = "Please select a Field Type")]
        public int FieldType
        {
            get 
            { 
                return fieldType; 
            }
            set 
            { 
                fieldType = value; 
            }
        }

        public string FieldTypeText
        {
            get 
            { 
                return fieldTypeText; 
            }
            set 
            { 
                fieldTypeText = value; 
            }
        }

        public int ValidationType
        {
            get 
            { 
                return validationType; 
            }
            set 
            { 
                validationType = value; 
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

        public List<FormField> Children
        {
            get 
            { 
                return children; 
            }
            set 
            { 
                children = value; 
            }
        }

        public int IsRequired
        {
            get 
            { 
                return isRequired; 
            }
            set 
            { 
                isRequired = value; 
            }
        }

        public FormField()
        {
            this.Children = new List<FormField>();
        }
    }
}