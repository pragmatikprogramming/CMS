using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Domain.Entities
{
    public class Trash
    {
        private int id;
        private int objectId;
        private string objectTable;
        private string objectName;
        private string objectColumn;
        private string objectType;
        private DateTime deleteDate;
        private int deletedBy;
        private string deletedByName;

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

        public string ObjectColumn
        {
            get 
            { 
                return objectColumn; 
            }
            set 
            { 
                objectColumn = value; 
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

        public DateTime DeleteDate
        {
            get 
            { 
                return deleteDate; 
            }
            set 
            { 
                deleteDate = value; 
            }
        }

        public int DeletedBy
        {
            get 
            { 
                return deletedBy; 
            }
            set 
            { 
                deletedBy = value; 
            }
        }

        public string DeletedByName
        {
            get 
            {   
                return deletedByName; 
            }
            set 
            { 
                deletedByName = value; 
            }
        }
    }
}