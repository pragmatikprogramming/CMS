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
        public static void Create (Event m_Event)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            queryString = "INSERT INTO CMS_Events(contentGroup, eventTitle, eventStartDate, eventEndDate, branch, body) VALUES(@contentGroup, @eventTitle, @eventStartDate, @eventEndDate, @branch, @body)";
            SqlCommand createEvent = new SqlCommand(queryString, conn);

            createEvent.Parameters.AddWithValue("contentGroup", m_Event.ContentGroup);
            createEvent.Parameters.AddWithValue("eventTitle", m_Event.EventTitle);
            createEvent.Parameters.AddWithValue("eventStartDate", DateTime.Parse(m_Event.EventStartDate.ToString("MM/dd/yyyy") + " " + m_Event.EventStartHour.ToString() + ":" + m_Event.EventStartMin.ToString() + " " + m_Event.AmpmStart));
            createEvent.Parameters.AddWithValue("eventEndDate", DateTime.Parse(m_Event.EventEndDate.ToString("MM/dd/yyyy") + " " + m_Event.EventEndHour.ToString() + ":" + m_Event.EventEndMin.ToString() + " " + m_Event.AmpmEnd));
            createEvent.Parameters.AddWithValue("branch", m_Event.Branch);
            createEvent.Parameters.AddWithValue("body", m_Event.Body);

            createEvent.ExecuteNonQuery();
        }
    }
}