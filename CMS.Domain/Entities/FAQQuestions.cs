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
        private int pageWorkFlowState;
        private int sortOrder;

        


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

        [Required(ErrorMessage = "Something terrible happened during validation.  Please contact your administrator")]
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

        public int SortOrder
        {
            get 
            { 
                return sortOrder; 
            }
            set 
            { 
                sortOrder = value; 
            }
        }
    }
}