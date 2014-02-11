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

            string queryString = "SELECT * FROM CMS_FAQs WHERE id = @id AND pageWorkFlowState != 4";
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

            string queryString = "SELECT * FROM CMS_FAQs WHERE pageWorkFlowState != 4";
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

            FAQ m_FAQ = DBFAQ.RetrieveOneFAQ(m_FaqID);

            string queryString = "UPDATE CMS_FAQs SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand deleteFAQ = new SqlCommand(queryString, conn);
            deleteFAQ.Parameters.AddWithValue("id", m_FaqID);
            deleteFAQ.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_FAQs', @objectName, @deleteDate, @deletedBy, 'id', 'FAQ')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_FAQ.FaqID);
            insertTrash.Parameters.AddWithValue("objectName", m_FAQ.FaqName);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();

            conn.Close();
        }


        //QUESTIONS CRUD FUNCTIONS

        public static void CreateFAQQuestion(FAQQuestions m_FAQQuestion)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            int sortOrder = getSortOrder(m_FAQQuestion.FaqID);

            string queryString = "INSERT INTO CMS_FAQQuestions(faqID, faqQuestion, faqAnswer, sortOrder) VALUES(@faqID, @faqQuestion, @faqAnswer, @sortOrder)";
            SqlCommand insertFAQQuestion = new SqlCommand(queryString, conn);
            insertFAQQuestion.Parameters.AddWithValue("faqID", m_FAQQuestion.FaqID);
            insertFAQQuestion.Parameters.AddWithValue("faqQuestion", m_FAQQuestion.FaqQuestion);
            insertFAQQuestion.Parameters.AddWithValue("faqAnswer", m_FAQQuestion.FaqAnswer);
            insertFAQQuestion.Parameters.AddWithValue("sortOrder", sortOrder);
            insertFAQQuestion.ExecuteNonQuery();

            conn.Close();
        }

        public static FAQQuestions RetrieveOneFAQQuestion(int m_FaqQuestionID)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FAQQuestions WHERE id = @id AND pageWorkFlowState != 4";
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
                tempQuestion.PageWorkFlowState = myFAQQuestion.GetInt32(4);
                tempQuestion.SortOrder = myFAQQuestion.GetInt32(5);
            }

            conn.Close();
            return tempQuestion;
        }

        public static List<FAQQuestions> RetrieveAllFAQQuestions(int m_FaqID)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FAQQuestions WHERE faqID = @faqID AND pageWorkFlowState != 4 ORDER BY sortOrder";
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

            FAQQuestions m_FAQQuestion = DBFAQ.RetrieveOneFAQQuestion(m_QID);

            string queryString = "UPDATE CMS_FAQQuestions SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand deleteFAQQuestion = new SqlCommand(queryString, conn);
            deleteFAQQuestion.Parameters.AddWithValue("id", m_QID);
            deleteFAQQuestion.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_FAQQuestions', @objectName, @deleteDate, @deletedBy, 'id', 'FAQ Question')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_FAQQuestion.FaqID);
            insertTrash.Parameters.AddWithValue("objectName", m_FAQQuestion.FaqQuestion);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();


            conn.Close();
        }

        public static int getSortOrder(int faqId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 sortOrder FROM CMS_FAQQuestions WHERE faqID = @faqId ORDER BY sortOrder DESC";
            SqlCommand getSortOrder = new SqlCommand(queryString, conn);
            getSortOrder.Parameters.AddWithValue("faqId", faqId);
            object sortOrder = getSortOrder.ExecuteScalar();
            int m_sortOrder = 0;

            if (sortOrder == null)
            {
                m_sortOrder = 1;
            }
            else
            {
                m_sortOrder = Convert.ToInt32(sortOrder);
                m_sortOrder++;
            }

            conn.Close();

            return m_sortOrder;
        }

        public static void sortUp(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            FAQQuestions m_FAQQuestion = DBFAQ.RetrieveOneFAQQuestion(id);
            int oldSortOrder = m_FAQQuestion.SortOrder;
            int newSortOrder = oldSortOrder - 1;

            //check boundaries of sort to make sure they are valid

            string queryString = "SELECT TOP 1 id FROM CMS_FAQQuestions WHERE faqID = @faqID and sortOrder = @sortOrder ORDER BY id DESC";
            SqlCommand getId = new SqlCommand(queryString, conn);
            getId.Parameters.AddWithValue("faqID", m_FAQQuestion.FaqID);
            getId.Parameters.AddWithValue("sortOrder", newSortOrder);

            object myId = getId.ExecuteScalar();

            if (myId != null)
            {
                int m_id = Convert.ToInt32(myId);
                FAQQuestions o_FAQQuestion = DBFAQ.RetrieveOneFAQQuestion(m_id);

                queryString = "UPDATE CMS_FAQQuestions SET sortOrder = @sortOrder WHERE id = @id";
                SqlCommand updatePage = new SqlCommand(queryString, conn);
                updatePage.Parameters.AddWithValue("sortOrder", newSortOrder);
                updatePage.Parameters.AddWithValue("id", m_FAQQuestion.QID);
                updatePage.ExecuteNonQuery();

                SqlCommand updatePage2 = new SqlCommand(queryString, conn);
                updatePage2.Parameters.AddWithValue("sortOrder", oldSortOrder);
                updatePage2.Parameters.AddWithValue("id", o_FAQQuestion.QID);
                updatePage2.ExecuteNonQuery();
            }

            conn.Close();
        }

        public static void sortDown(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            FAQQuestions m_FAQQuestion = DBFAQ.RetrieveOneFAQQuestion(id);
            int oldSortOrder = m_FAQQuestion.SortOrder;
            int newSortOrder = oldSortOrder + 1;

            //check boundaries of sort to make sure they are valid

            string queryString = "SELECT id FROM CMS_FAQQuestions WHERE faqID = @faqID and sortOrder = @sortOrder ORDER BY id DESC";
            SqlCommand getId = new SqlCommand(queryString, conn);
            getId.Parameters.AddWithValue("faqID", m_FAQQuestion.FaqID);
            getId.Parameters.AddWithValue("sortOrder", newSortOrder);

            object myId = getId.ExecuteScalar();

            if (myId != null)
            {
                int m_id = Convert.ToInt32(myId);
                FAQQuestions o_FAQQuestion = DBFAQ.RetrieveOneFAQQuestion(m_id);

                queryString = "UPDATE CMS_FAQQuestions SET sortOrder = @sortOrder, pageWorkFlowState = 1 WHERE id = @id";
                SqlCommand updatePage = new SqlCommand(queryString, conn);
                updatePage.Parameters.AddWithValue("sortOrder", newSortOrder);
                updatePage.Parameters.AddWithValue("id", m_FAQQuestion.QID);
                updatePage.ExecuteNonQuery();

                SqlCommand updatePage2 = new SqlCommand(queryString, conn);
                updatePage2.Parameters.AddWithValue("sortOrder", oldSortOrder);
                updatePage2.Parameters.AddWithValue("id", o_FAQQuestion.QID);
                updatePage2.ExecuteNonQuery();
            }

            conn.Close();
        }
    }
}