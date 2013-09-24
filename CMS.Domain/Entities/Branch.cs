using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Domain.Entities
{
    public class Branch
    {
        private int id;
        private string branchName;

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

        public string BranchName
        {
            get 
            { 
                return branchName; 
            }
            set 
            { 
                branchName = value; 
            }
        }
    }
}