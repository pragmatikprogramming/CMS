using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBEvent
    {
        public static Event RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            string action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            if (action == "PagePreview")
            {
                queryString = "SELECT * FROM CMS_Events WHERE id = @id";
            }
            else
            {
                queryString = "SELECT * FROM CMS_Events WHERE id = @id AND pageWorkFlowState != 4";
            }
            
            SqlCommand getEvent = new SqlCommand(queryString, conn);

            getEvent.Parameters.AddWithValue("id", id);

            SqlDataReader myEvent = getEvent.ExecuteReader();

            Event m_Event = new Event();

            if (myEvent.Read())
            {
                m_Event.EventID = myEvent.GetInt32(0);
                m_Event.ContentGroup = myEvent.GetInt32(1);
                m_Event.EventTitle = myEvent.GetString(2);
                m_Event.EventStartDate = myEvent.GetDateTime(3);
                m_Event.EventEndDate = myEvent.GetDateTime(4);
                m_Event.Branch = myEvent.GetInt32(5);
                m_Event.BranchName = Utility.getBranchName(m_Event.Branch);
                m_Event.Body = myEvent.GetString(6);
                m_Event.PageWorkFlowState = myEvent.GetInt32(7);
                m_Event.LockedBy = myEvent.GetInt32(8);
                m_Event.FeaturedEvent = myEvent.GetInt32(11);

                if(m_Event.EventStartDate.Hour >= 12)
                {
                    if (m_Event.EventStartDate.Hour == 12)
                    {
                        m_Event.EventStartHour = 12;
                    }
                    else
                    {
                        m_Event.EventStartHour = m_Event.EventStartDate.Hour % 12;
                    }

                    m_Event.AmpmStart = "pm";
                }
                else
                {
                    m_Event.AmpmStart = "am";
                    m_Event.EventStartHour = m_Event.EventStartDate.Hour;
                }
                if(m_Event.EventEndDate.Hour >= 12)
                {
                    if (m_Event.EventEndDate.Hour == 12)
                    {
                        m_Event.EventEndHour = 12;
                    }
                    else
                    {
                        m_Event.EventEndHour = m_Event.EventEndDate.Hour % 12;
                    }

                    m_Event.AmpmEnd = "pm";
                }
                else
                {
                    m_Event.AmpmEnd = "am";
                    m_Event.EventEndHour = m_Event.EventEndDate.Hour;
                }

                m_Event.EventStartMin = m_Event.EventStartDate.Minute;
                m_Event.EventEndMin = m_Event.EventEndDate.Minute;

            }

            conn.Close();
            return m_Event;
        }

        public static void Update(Event m_Event)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Events SET contentGroup = @contentGroup, eventTitle = @eventTitle, eventStartDate = @eventStartDate, eventEndDate = @eventEndDate, branch = @branch, body = @body, pageWorkFlowState = 1, lockedBy = @lockedBy, lastModifiedBy = @lastModifiedBy, lastModifiedDate = @lastModifiedDate, featuredEvent = @featuredEvent WHERE id = @EventID";
            SqlCommand updateEvent = new SqlCommand(queryString, conn);

            string myStartTime = string.Empty;
            string myEndTime = string.Empty;

            if (m_Event.EventStartHour != -1 && m_Event.EventStartMin != -1 && m_Event.AmpmStart != "-1")
            {
                myStartTime = " " + m_Event.EventStartHour.ToString() + ":" + m_Event.EventStartMin.ToString() + " " + m_Event.AmpmStart;
            }
            if (m_Event.EventEndHour != -1 && m_Event.EventEndMin != -1 && m_Event.AmpmEnd != "-1")
            {
                myEndTime = " " + m_Event.EventEndHour.ToString() + ":" + m_Event.EventEndMin.ToString() + " " + m_Event.AmpmEnd;
            }

            updateEvent.Parameters.AddWithValue("contentGroup", m_Event.ContentGroup);
            updateEvent.Parameters.AddWithValue("eventTitle", m_Event.EventTitle);
            updateEvent.Parameters.AddWithValue("eventStartDate", DateTime.Parse(m_Event.EventStartDate.ToString("MM/dd/yyyy") + myStartTime));
            updateEvent.Parameters.AddWithValue("eventEndDate", DateTime.Parse(m_Event.EventEndDate.ToString("MM/dd/yyyy") + myEndTime));
            updateEvent.Parameters.AddWithValue("branch", m_Event.Branch);
            updateEvent.Parameters.AddWithValue("body", m_Event.Body);
            updateEvent.Parameters.AddWithValue("EventID", m_Event.EventID);
            updateEvent.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
            updateEvent.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
            updateEvent.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
            updateEvent.Parameters.AddWithValue("featuredEvent", m_Event.FeaturedEvent);

            updateEvent.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            Event m_Event = DBEvent.RetrieveOne(id);

            string queryString;
            queryString = "UPDATE CMS_Events SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand deleteEvent = new SqlCommand(queryString, conn);

            deleteEvent.Parameters.AddWithValue("id", id);
            deleteEvent.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_Events', @objectName, @deleteDate, @deletedBy, 'id', 'Calendar')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_Event.EventID);
            insertTrash.Parameters.AddWithValue("objectName", m_Event.EventTitle);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);

            insertTrash.ExecuteNonQuery();

            conn.Close();
        }

        public static void Create (Event m_Event)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            queryString = "INSERT INTO CMS_Events(contentGroup, eventTitle, eventStartDate, eventEndDate, branch, body, pageWorkFlowState, lockedBy, lastModifiedBy, lastModifiedDate, featuredEvent) VALUES(@contentGroup, @eventTitle, @eventStartDate, @eventEndDate, @branch, @body, 1, @lockedBy, @lastModifiedBy, @lastModifiedDate, @featuredEvent)";
            SqlCommand createEvent = new SqlCommand(queryString, conn);

            string myStartTime = string.Empty;
            string myEndTime = string.Empty;

            if (m_Event.EventStartHour != -1 && m_Event.EventStartMin != -1 && m_Event.AmpmStart != "-1")
            {
                myStartTime = " " + m_Event.EventStartHour.ToString() + ":" + m_Event.EventStartMin.ToString() + " " + m_Event.AmpmStart;
            }
            if (m_Event.EventEndHour != -1 && m_Event.EventEndMin != -1 && m_Event.AmpmEnd != "-1")
            {
                myEndTime = " " + m_Event.EventEndHour.ToString() + ":" + m_Event.EventEndMin.ToString() + " " + m_Event.AmpmEnd;
            }
            createEvent.Parameters.AddWithValue("contentGroup", m_Event.ContentGroup);
            createEvent.Parameters.AddWithValue("eventTitle", m_Event.EventTitle);
            createEvent.Parameters.AddWithValue("eventStartDate", DateTime.Parse(m_Event.EventStartDate.ToString("MM/dd/yyyy") + myStartTime));
            createEvent.Parameters.AddWithValue("eventEndDate", DateTime.Parse(m_Event.EventEndDate.ToString("MM/dd/yyyy") + myEndTime));
            createEvent.Parameters.AddWithValue("branch", m_Event.Branch);
            createEvent.Parameters.AddWithValue("body", m_Event.Body);
            createEvent.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
            createEvent.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
            createEvent.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
            createEvent.Parameters.AddWithValue("featuredEvent", m_Event.FeaturedEvent);
            

            createEvent.ExecuteNonQuery();

            conn.Close();
        }

        public static List<Event> RetrieveAll(DateTime searchDate)
        {
            List<Event> myEvents = new List<Event>();

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            queryString = "SELECT id, eventTitle, pageWorkFlowState, lockedBy from CMS_Events WHERE @searchDateStart >= eventStartDate AND @searchDate <= eventEndDate AND pageWorkFlowState != 4";
            SqlCommand getEvent = new SqlCommand(queryString, conn);
            getEvent.Parameters.AddWithValue("searchDateStart", DateTime.Parse(searchDate.ToString("MM/dd/yyyy") + " 23:59:59"));
            getEvent.Parameters.AddWithValue("searchDate", DateTime.Parse(searchDate.ToString("MM/dd/yyyy") + " 00:00:00"));

            SqlDataReader eventReader = getEvent.ExecuteReader();

            while (eventReader.Read())
            {
                Event tempEvent = new Event();
                tempEvent.EventID = eventReader.GetInt32(0);
                tempEvent.EventTitle = eventReader.GetString(1);
                tempEvent.PageWorkFlowState = eventReader.GetInt32(2);
                tempEvent.LockedBy = eventReader.GetInt32(3);
                myEvents.Add(tempEvent);
            }

            conn.Close();
            return myEvents;
        }

        public static void LockEvent(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Events SET lockedBy = @lockedBy WHERE id = @id";
            SqlCommand updateEvent = new SqlCommand(queryString, conn);
            updateEvent.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
            updateEvent.Parameters.AddWithValue("id", id);
            updateEvent.ExecuteNonQuery();

            conn.Close();
        }

        public static void UnlockEvent(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Events SET lockedBy = 0 WHERE id = @id";
            SqlCommand updateEvent = new SqlCommand(queryString, conn);
            updateEvent.Parameters.AddWithValue("id", id);
            updateEvent.ExecuteNonQuery();

            conn.Close();
        }

        public static void PublishEvent(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Events SET pageWorkFlowState = 2, lockedBy = 0 WHERE id = @id";
            SqlCommand updateEvent = new SqlCommand(queryString, conn);
            updateEvent.Parameters.AddWithValue("id", id);
            updateEvent.ExecuteNonQuery();


            conn.Close();
        }
        public static List<Branch> BranchNames()
        {
            List<Branch> m_Branchs = new List<Branch>();

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_BranchNames ORDER BY BranchName";
            SqlCommand getBranchNames = new SqlCommand(queryString, conn);

            SqlDataReader BranchNames = getBranchNames.ExecuteReader();

            while (BranchNames.Read())
            {
                Branch m_Branch = new Branch();
                m_Branch.Id = BranchNames.GetInt32(0);
                m_Branch.BranchName = BranchNames.GetString(1);
                m_Branchs.Add(m_Branch);
            }
            
            conn.Close();
            return m_Branchs;
        }

        public static List<ContentGroup> ContentGroups()
        {
            List<ContentGroup> m_ContentGroups = new List<ContentGroup>();

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_ContentGroups WHERE id != 1 ORDER BY ContentGroup";
            SqlCommand getContentGroups = new SqlCommand(queryString, conn);

            SqlDataReader ContentGroups = getContentGroups.ExecuteReader();

            while (ContentGroups.Read())
            {
                ContentGroup m_ContentGroup = new ContentGroup();
                m_ContentGroup.GroupID = ContentGroups.GetInt32(0);
                m_ContentGroup.ContentGroupName = ContentGroups.GetString(1);
                m_ContentGroups.Add(m_ContentGroup);
            }

            conn.Close();
            return m_ContentGroups;
        }

        public static List<Event> getFeaturedEvents()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 5 * FROM CMS_Events WHERE featuredEvent = 1 AND eventEndDate >= @startDate AND pageWorkFlowState = 2 ORDER BY eventStartDate ASC";
            SqlCommand getEvents = new SqlCommand(queryString, conn);
            getEvents.Parameters.AddWithValue("startDate", DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy") + " 00:00:00"));
            //getEvents.Parameters.AddWithValue("endDate", DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")));

            SqlDataReader eventReader = getEvents.ExecuteReader();

            List<Event> myEvents = new List<Event>();

            while (eventReader.Read())
            {
                Event tempEvent = new Event();
                tempEvent.EventID = eventReader.GetInt32(0);
                tempEvent.EventTitle = eventReader.GetString(2);
                tempEvent.PageWorkFlowState = eventReader.GetInt32(7);
                tempEvent.LockedBy = eventReader.GetInt32(8);

                DateTime startDate = eventReader.GetDateTime(3);
                DateTime endDate = eventReader.GetDateTime(4);

                tempEvent.EventStartDate = DateTime.Parse(startDate.ToString("MM/dd/yyyy"));
                tempEvent.EventEndDate = DateTime.Parse(endDate.ToString("MM/dd/yyyy"));
                tempEvent.EventStartHour = startDate.Hour % 12;
                tempEvent.EventStartMin = startDate.Minute;
                tempEvent.EventEndHour = endDate.Hour % 12;
                tempEvent.EventEndMin = endDate.Minute;

                if (startDate.Hour >= 12)
                {
                    tempEvent.AmpmStart = "PM";
                }
                else
                {
                    tempEvent.AmpmStart = "AM";
                }

                if (endDate.Hour >= 12)
                {
                    tempEvent.AmpmEnd = "PM";
                }
                else
                {
                    tempEvent.AmpmEnd = "AM";
                }

                tempEvent.Branch = eventReader.GetInt32(5);
                tempEvent.BranchName = Utility.getBranchName(tempEvent.Branch);

                myEvents.Add(tempEvent);
            }

            conn.Close();

            return myEvents;
        }

        public static Event getTopByEventId(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 * FROM CMS_Events WHERE id = @id ORDER BY id DESC";
            SqlCommand getEvent = new SqlCommand(queryString, conn);
            getEvent.Parameters.AddWithValue("id", id);
            SqlDataReader myEvent = getEvent.ExecuteReader();

            Event m_Event = new Event();

            if (myEvent.Read())
            {
                m_Event.EventID = myEvent.GetInt32(0);
                m_Event.ContentGroup = myEvent.GetInt32(1);
                m_Event.EventTitle = myEvent.GetString(2);
                m_Event.EventStartDate = myEvent.GetDateTime(3);
                m_Event.EventEndDate = myEvent.GetDateTime(4);
                m_Event.Branch = myEvent.GetInt32(5);
                m_Event.BranchName = Utility.getBranchName(m_Event.Branch);
                m_Event.Body = myEvent.GetString(6);
                m_Event.PageWorkFlowState = myEvent.GetInt32(7);
                m_Event.LockedBy = myEvent.GetInt32(8);
                m_Event.FeaturedEvent = myEvent.GetInt32(11);

                if (m_Event.EventStartDate.Hour >= 12)
                {
                    if (m_Event.EventStartDate.Hour == 12)
                    {
                        m_Event.EventStartHour = 12;
                    }
                    else
                    {
                        m_Event.EventStartHour = m_Event.EventStartDate.Hour % 12;
                    }

                    m_Event.AmpmStart = "pm";
                }
                else
                {
                    m_Event.AmpmStart = "am";
                    m_Event.EventStartHour = m_Event.EventStartDate.Hour;
                }
                if (m_Event.EventEndDate.Hour >= 12)
                {
                    if (m_Event.EventEndDate.Hour == 12)
                    {
                        m_Event.EventEndHour = 12;
                    }
                    else
                    {
                        m_Event.EventEndHour = m_Event.EventEndDate.Hour % 12;
                    }

                    m_Event.AmpmEnd = "pm";
                }
                else
                {
                    m_Event.AmpmEnd = "am";
                    m_Event.EventEndHour = m_Event.EventEndDate.Hour;
                }

                m_Event.EventStartMin = m_Event.EventStartDate.Minute;
                m_Event.EventEndMin = m_Event.EventEndDate.Minute;

            }

            conn.Close();
            return m_Event;

        }
    }
}