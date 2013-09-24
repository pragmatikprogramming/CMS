using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class FAQQuestions
    {
        private int qID;
        private int faqID;
        private string faqQuestion;
        private string faqAnswer;

        public int QID
        {
            get 
            { 
                return qID; 
            }
            set 
            { 
                qID = value; 
            }
        }

        [Required(ErrorMessage = "Something terrible happened during validatin.  Please contact your administrator")]
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

        [Required(ErrorMessage = "Please enter a Question")]
        public string FaqQuestion
        {
            get 
            { 
                return faqQuestion; 
            }
            set 
            { 
                faqQuestion = value; 
            }
        }

        [Required(ErrorMessage = "Please enter an Answer")]
        public string FaqAnswer
        {
            get 
            { 
                return faqAnswer; 
            }
            set 
            { 
                faqAnswer = value;
            }
        }
    }
}