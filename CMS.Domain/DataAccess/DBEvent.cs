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
        public static Event RetrieveOne(string id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            queryString = "SELECT * FROM CMS_Events WHERE id = @id";
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
                m_Event.Body = myEvent.GetString(6);

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

            string queryString = "UPDATE CMS_Events SET contentGroup = @contentGroup, eventTitle = @eventTitle, eventStartDate = @eventStartDate, eventEndDate = @eventEndDate, branch = @branch, body = @body WHERE id = @EventID";
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

            updateEvent.ExecuteNonQuery();

            conn.Close();
        }

        public static void Delete(string id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            queryString = "DELETE FROM CMS_Events WHERE id = @id";
            SqlCommand deleteEvent = new SqlCommand(queryString, conn);

            deleteEvent.Parameters.AddWithValue("id", id);
            deleteEvent.ExecuteNonQuery();

            conn.Close();
        }

        public static void Create (Event m_Event)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            queryString = "INSERT INTO CMS_Events(contentGroup, eventTitle, eventStartDate, eventEndDate, branch, body) VALUES(@contentGroup, @eventTitle, @eventStartDate, @eventEndDate, @branch, @body)";
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

            createEvent.ExecuteNonQuery();

            conn.Close();
        }

        public static List<Event> RetrieveAll(DateTime searchDate)
        {
            List<Event> myEvents = new List<Event>();

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            queryString = "SELECT id, eventTitle from CMS_Events WHERE @searchDate >= eventStartDate AND @searchDate <= eventEndDate";
            SqlCommand getEvent = new SqlCommand(queryString, conn);
            
            getEvent.Parameters.AddWithValue("searchDate", searchDate);

            SqlDataReader eventReader = getEvent.ExecuteReader();

            while (eventReader.Read())
            {
                Event tempEvent = new Event();
                tempEvent.EventID = eventReader.GetInt32(0);
                tempEvent.EventTitle = eventReader.GetString(1);
                myEvents.Add(tempEvent);
            }

            conn.Close();
            return myEvents;
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
    }
}