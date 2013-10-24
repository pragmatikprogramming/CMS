using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IFAQRepository
    {
        bool CreateFAQ(FAQ m_FAQ);
        FAQ RetrieveOneFAQ(int m_FaqID);
        List<FAQ> RetrieveAllFAQ();
        bool UpdateFAQ(FAQ m_FAQ);
        bool DeleteFAQ(int m_FaqID);

        bool CreateFAQQuestion(FAQQuestions m_FAQQuestion);
        FAQQuestions RetrieveOneFAQQuestion(int m_FaqQuestionID);
        List<FAQQuestions> RetrieveAllFAQQuestions(int FaqID);
        bool UpdateFAQQuestion(FAQQuestions m_FAQQuestion);
        bool DeleteFAQQuestion(int m_FaqQuestionID);
        void sortUp(int id);
        void sortDown(int id);
    }
}
