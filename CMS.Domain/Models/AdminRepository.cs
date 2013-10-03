using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class AdminRepository : IAdminRepository
    {
        public List<Admin> getAwaitingApproval(int pageNum)
        {
            List<Admin> m_List = DBAdmin.getAwaitingApproval(pageNum);
            return m_List;
        }

        public List<Admin> getLockedContent(int pageNum)
        {
            List<Admin> m_List = DBAdmin.getLockedContent(pageNum);
            return m_List;
        }

        public List<Trash> getTrash(int pageNum)
        {
            List<Trash> m_List = new List<Trash>();
            return m_List;
        }

        public void TrashRestore(int id)
        {
            Trash m_Trash = DBTrash.RetrieveOne(id);
            DBTrash.Restore(m_Trash);
        }

        public void UnlockContent(string objectType, int id)
        {
            if (objectType == "Page")
            {
                DBPage.unlockPage(id);
            }
            else if (objectType == "Calendar")
            {
                DBEvent.UnlockEvent(id);
            }
        }
    }
}