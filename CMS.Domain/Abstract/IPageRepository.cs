using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IPageRepository
    {
        void Create(Page m_Page);
        Page RetrieveOne(int m_Id);
        Page RetrieveOneByFriendlyURL(string friendlyURL);
        List<Page> RetrieveAll(int m_Id);
        void Update(Page m_Page);
        bool TrashCan(int m_Id);
        void Delete(int m_Id);
        int Publish(int m_Id);
        void LockPage(int pid);
        void UnlockPage(int pid);
        void sortUp(int id);
        void sortDown(int id);
        bool friendlyURLExists(string friendlyURL, int pageId);
    }
}
