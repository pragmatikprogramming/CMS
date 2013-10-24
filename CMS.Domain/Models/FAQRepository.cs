using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class FAQRepository : IFAQRepository
    {
        public bool CreateFAQ(FAQ m_FAQ)
        {
            DBFAQ.CreateFAQ(m_FAQ);
            return true;
        }

        public FAQ RetrieveOneFAQ(int m_FaqID)
        {
            FAQ m_FAQ = DBFAQ.RetrieveOneFAQ(m_FaqID);
            return m_FAQ;
        }

        public List<FAQ> RetrieveAllFAQ()
        {
            List<FAQ> m_FAQS = DBFAQ.RetrieveAllFAQ();
            return m_FAQS;
        }

        public bool UpdateFAQ(FAQ m_FAQ)
        {
            DBFAQ.UpdateFAQ(m_FAQ);
            return true;
        }

        public bool DeleteFAQ(int m_FaqID)
        {
            DBFAQ.DeleteFAQ(m_FaqID);
            return true;
        }

        public bool CreateFAQQuestion(FAQQuestions m_FAQQuestion)
        {
            DBFAQ.CreateFAQQuestion(m_FAQQuestion);

            return true;
        }

        public FAQQuestions RetrieveOneFAQQuestion(int m_FAQQuestionID)
        {
            FAQQuestions m_FAQQuestion = DBFAQ.RetrieveOneFAQQuestion(m_FAQQuestionID);
            return m_FAQQuestion;
        }

        public List<FAQQuestions> RetrieveAllFAQQuestions(int FaqID)
        {
            List<FAQQuestions> m_FAQQuestions = DBFAQ.RetrieveAllFAQQuestions(FaqID);
            return m_FAQQuestions;
        }

        public bool UpdateFAQQuestion(FAQQuestions m_FAQQuestion)
        {
            DBFAQ.UpdateFAQQuestion(m_FAQQuestion);
            return true;
        }

        public bool DeleteFAQQuestion(int m_FAQQuestionID)
        {
            DBFAQ.DeleteFAQQuestion(m_FAQQuestionID);

            return true;
        }

        public void sortUp(int id)
        {
            DBFAQ.sortUp(id);
        }

        public void sortDown(int id)
        {
            DBFAQ.sortDown(id);
        }
    }
}