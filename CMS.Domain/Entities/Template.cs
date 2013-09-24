using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Domain.Entities
{
    public class Template
    {
        private int templateId;
        private string templateName;

        public int TemplateId
        {
            get 
            { 
                return templateId; 
            }
            set 
            { 
                templateId = value; 
            }
        }

        public string TemplateName
        {
            get 
            { 
                return templateName; 
            }
            set 
            { 
                templateName = value; 
            }
        }
    }
}