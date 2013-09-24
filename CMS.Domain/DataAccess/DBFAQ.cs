using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;


namespace CMS.Domain.DataAccess
{
    public class DBFAQ
    {
        public static void CreateFAQ(FAQ m_FAQ)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_FAQs(faqName, contentGroup) VALUES(@faqName, @contentGroup)";
            SqlCommand insertFAQ = new SqlCommand(queryString, conn);
            insertFAQ.Parameters.AddWithValue("faqName", m_FAQ.FaqName);
            insertFAQ.Parameters.AddWithValue("contentGroup", m_FAQ.ContentGroup);
            insertFAQ.ExecuteNonQuery();

            conn.Close();

        }

        public static FAQ RetrieveOneFAQ(int m_FaqID)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FAQs WHERE id = @id";
            SqlCommand getFAQ = new SqlCommand(queryString, conn);
            getFAQ.Parameters.AddWithValue("id", m_FaqID);

            SqlDataReader myFAQ = getFAQ.ExecuteReader();

            FAQ tempFAQ = new FAQ();

            if (myFAQ.Read())
            {
                tempFAQ.FaqID = myFAQ.GetInt32(0);
                tempFAQ.FaqName = myFAQ.GetString(1);
                tempFAQ.ContentGroup = myFAQ.GetInt32(2);
            }

            conn.Close();
            return tempFAQ;
        }
        public static List<FAQ> RetrieveAllFAQ()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FAQs";
            SqlCommand getFAQs = new SqlCommand(queryString, conn);
            SqlDataReader m_FAQs = getFAQs.ExecuteReader();

            List<FAQ> myFAQs = new List<FAQ>();

            while (m_FAQs.Read())
            {
                FAQ tempFAQ = new FAQ();
                tempFAQ.FaqID = m_FAQs.GetInt32(0);
                tempFAQ.FaqName = m_FAQs.GetString(1);
                tempFAQ.ContentGroup = m_FAQs.GetInt32(2);
                myFAQs.Add(tempFAQ);
            }

            conn.Close();
            return myFAQs;
        }

        public static void UpdateFAQ(FAQ m_FAQ)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_FAQs SET faqName = @faqName, contentGroup = @contentGroup WHERE id = @id";
            SqlCommand updateFAQ = new SqlCommand(queryString, conn);
            updateFAQ.Parameters.AddWithValue("faqName", m_FAQ.FaqName);
            updateFAQ.Parameters.AddWithValue("contentGroup", m_FAQ.ContentGroup);
            updateFAQ.Parameters.AddWithValue("id", m_FAQ.FaqID);
            updateFAQ.ExecuteNonQuery();

            conn.Close();
        }

        public static void DeleteFAQ(int m_FaqID)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM CMS_FAQs WHERE id = @id";
            SqlCommand deleteFAQ = new SqlCommand(queryString, conn);
            deleteFAQ.Parameters.AddWithValue("id", m_FaqID);
            deleteFAQ.ExecuteNonQuery();

            conn.Close();
        }


        //QUESTIONS CRUD FUNCTIONS

        public static void CreateFAQQuestion(FAQQuestions m_FAQQuestion)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_FAQQuestions(faqID, faqQuestion, faqAnswer) VALUES(@faqID, @faqQuestion, @faqAnswer)";
            SqlCommand insertFAQQuestion = new SqlCommand(queryString, conn);
            insertFAQQuestion.Parameters.AddWithValue("faqID", m_FAQQuestion.FaqID);
            insertFAQQuestion.Parameters.AddWithValue("faqQuestion", m_FAQQuestion.FaqQuestion);
            insertFAQQuestion.Parameters.AddWithValue("faqAnswer", m_FAQQuestion.FaqAnswer);
            insertFAQQuestion.ExecuteNonQuery();

            conn.Close();
        }

        public static FAQQuestions RetrieveOneFAQQuestion(int m_FaqQuestionID)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FAQQuestions WHERE id = @id";
            SqlCommand getFAQQuestion = new SqlCommand(queryString, conn);
            getFAQQuestion.Parameters.AddWithValue("id", m_FaqQuestionID);
            SqlDataReader myFAQQuestion = getFAQQuestion.ExecuteReader();

            FAQQuestions tempQuestion = new FAQQuestions();

            if (myFAQQuestion.Read())
            {
                tempQuestion.QID = myFAQQuestion.GetInt32(0);
                tempQuestion.FaqID = myFAQQuestion.GetInt32(1);
                tempQuestion.FaqQuestion = myFAQQuestion.GetString(2);
                tempQuestion.FaqAnswer = myFAQQuestion.GetString(3);
            }

            conn.Close();
            return tempQuestion;
        }

        public static List<FAQQuestions> RetrieveAllFAQQuestions(int m_FaqID)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FAQQuestions WHERE faqID = @faqID";
            SqlCommand getFAQQuestions = new SqlCommand(queryString, conn);
            getFAQQuestions.Parameters.AddWithValue("faqID", m_FaqID);

            SqlDataReader myFAQQuestions = getFAQQuestions.ExecuteReader();

            List<FAQQuestions> m_FAQs = new List<FAQQuestions>();

            while (myFAQQuestions.Read())
            {
                FAQQuestions tempFAQQuestion = new FAQQuestions();
                tempFAQQuestion.QID = myFAQQuestions.GetInt32(0);
                tempFAQQuestion.FaqID = myFAQQuestions.GetInt32(1);
                tempFAQQuestion.FaqQuestion = myFAQQuestions.GetString(2);
                tempFAQQuestion.FaqAnswer = myFAQQuestions.GetString(3);

                m_FAQs.Add(tempFAQQuestion);
            }

            conn.Close();
            return m_FAQs;
        }

        public static void UpdateFAQQuestion(FAQQuestions m_FAQQuestion)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_FAQQuestions SET faqID = @faqID, faqQuestion = @faqQuestion, faqAnswer = @faqAnswer WHERE id = @id";
            SqlCommand updateFAQQuestion = new SqlCommand(queryString, conn);
            updateFAQQuestion.Parameters.AddWithValue("faqID", m_FAQQuestion.FaqID);
            updateFAQQuestion.Parameters.AddWithValue("faqQuestion", m_FAQQuestion.FaqQuestion);
            updateFAQQuestion.Parameters.AddWithValue("faqAnswer", m_FAQQuestion.FaqAnswer);
            updateFAQQuestion.Parameters.AddWithValue("id", m_FAQQuestion.QID);
            updateFAQQuestion.ExecuteNonQuery();

            conn.Close();
        }

        public static void DeleteFAQQuestion(int m_QID)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM CMS_FAQQuestions WHERE id = @id";
            SqlCommand deleteFAQQuestion = new SqlCommand(queryString, conn);
            deleteFAQQuestion.Parameters.AddWithValue("id", m_QID);
            deleteFAQQuestion.ExecuteNonQuery();

            conn.Close();
        }
    }
}