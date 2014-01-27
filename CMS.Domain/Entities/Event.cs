using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Event
    {
        private int eventID;
        private int contentGroup;
        private string eventTitle;
        private DateTime eventStartDate;
        private int eventStartHour;
        private int eventStartMin;
        private string ampmStart;
        private DateTime eventEndDate;
        private int eventEndHour;
        private int eventEndMin;
        private string ampmEnd;
        private int branch;
        private string branchName;
        private string body;
        private int pageWorkFlowState;
        private int lockedBy;
        private int lastModifiedBy;
        private DateTime lastModifiedDate;
        private int featuredEvent;




        public int EventID
        {
            get 
            { 
                return eventID; 
            }
            set
            {
                eventID = value;
            }
        }

        [Required(ErrorMessage = "Please select a Content Group")]
        public int ContentGroup
        {
            get 
            { 
                return contentGroup; 
            }
            set 
            { 
                contentGroup = value; 
            }
        }

        [Required(ErrorMessage = "Please enter a Title for your Event")]
        public string EventTitle
        {
            get 
            { 
                return eventTitle; 
            }
            set 
            { 
                eventTitle = value; 
            }
        }

        [Required(ErrorMessage = "Please enter a Start Date")]
        public DateTime EventStartDate
        {
            get 
            { 
                return eventStartDate; 
            }
            set 
            { 
                eventStartDate = value; 
            }
        }

        public DateTime EventEndDate
        {
            get 
            { 
                return eventEndDate; 
            }
            set 
            { 
                eventEndDate = value; 
            }
        }

        [Required(ErrorMessage = "Body is a required field")]
        public string Body
        {
            get 
            { 
                return body; 
            }
            set 
            { 
                body = value; 
            }
        }

        [Required(ErrorMessage = "Please enter a Branch")]
        public int Branch
        {
            get 
            { 
                return branch; 
            }
            set 
            { 
                branch = value; 
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

        public int EventStartHour
        {
            get 
            { 
                return eventStartHour; 
            }
            set 
            { 
                eventStartHour = value; 
            }
        }

        public int EventStartMin
        {
            get 
            { 
                return eventStartMin; 
            }
            set 
            { 
                eventStartMin = value; 
            }
        }

        public string AmpmStart
        {
            get 
            { 
                return ampmStart; 
            }
            set 
            { 
                ampmStart = value; 
            }
        }

        public int EventEndHour
        {
            get 
            { 
                return eventEndHour; 
            }
            set 
            { 
                eventEndHour = value; 
            }
        }

        public int EventEndMin
        {
            get 
            { 
                return eventEndMin; 
            }
            set 
            { 
                eventEndMin = value; 
            }
        }

        public string AmpmEnd
        {
            get 
            { 
                return ampmEnd; 
            }
            set 
            { 
                ampmEnd = value; 
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

        public int FeaturedEvent
        {
            get 
            { 
                return featuredEvent; 
            }
            set 
            { 
                featuredEvent = value; 
            }
        }
    }
}