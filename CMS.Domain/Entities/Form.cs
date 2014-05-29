using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Form
    {
        private int id;
        private string formName;
        private string submissionEmail;
        private List<int> myFormFields;
        private List<FormField> formFields;
        private string success;

       
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

        [Required(ErrorMessage = "Please name your Form")]
        public string FormName
        {
            get 
            { 
                return formName; 
            }
            set 
            { 
                formName = value; 
            }
        }

        [Required(ErrorMessage = "Please enter an email to send results to.")]
        public string SubmissionEmail
        {
            get 
            { 
                return submissionEmail; 
            }
            set 
            { 
                submissionEmail = value; 
            }
        }

        [Required(ErrorMessage = "At least one form field is required")]
        public List<int> MyFormFields
        {
            get 
            { 
                return myFormFields; 
            }
            set 
            { 
                myFormFields = value; 
            }
        }

        public List<FormField> FormFields
        {
            get 
            { 
                return formFields; 
            }
            set 
            { 
                formFields = value; 
            }
        }

        [Required(ErrorMessage = "A success message is required")]
        public string Success
        {
            get 
            { 
                return success; 
            }
            set 
            { 
                success = value; 
            }
        }

        public Form()
        {
            FormFields = new List<FormField>();
        }

    }
}