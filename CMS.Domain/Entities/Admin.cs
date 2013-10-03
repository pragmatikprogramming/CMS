using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Domain.Entities
{
    public class Admin
    {
        private int objectId;
        private string objectName;
        private string objectType;
        private string objectTable;
        private int lockedBy;
        private string lockedByName;
        private int lastModifiedBy;
        private string lastModifiedName;
        private DateTime lastModifiedDate;


        public int ObjectId
        {
            get 
            { 
                return objectId; 
            }
            set 
            { 
                objectId = value; 
            }
        }

        public string ObjectName
        {
            get 
            { 
                return objectName; 
            }
            set 
            { 
                objectName = value; 
            }
        }

        public string ObjectType
        {
            get 
            { 
                return objectType; 
            }
            set 
            { 
                objectType = value; 
            }
        }

        public string ObjectTable
        {
            get 
            { 
                return objectTable; 
            }
            set 
            { 
                objectTable = value; 
            }
        }

        public int LockedBy
        {
            get 
            { 
                return lockedBy; 
            }
            set 
            { 
                lockedBy = value; 
            }
        }

        public string LockedByName
        {
            get 
            { 
                return lockedByName; 
            }
            set 
            { 
                lockedByName = value; 
            }
        }

        public int LastModifiedBy
        {
            get 
            { 
                return lastModifiedBy; 
            }
            set 
            { 
                lastModifiedBy = value; 
            }
        }

        public string LastModifiedName
        {
            get 
            { 
                return lastModifiedName; 
            }
            set 
            { 
                lastModifiedName = value; 
            }
        }

        public DateTime LastModifiedDate
        {
            get 
            { 
                return lastModifiedDate; 
            }
            set 
            { 
                lastModifiedDate = value; 
            }
        }
    }
}