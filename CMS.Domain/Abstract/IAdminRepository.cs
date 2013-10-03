using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IAdminRepository
    {
        List<Admin> getAwaitingApproval(int pageNum);
        List<Admin> getLockedContent(int pageNum);
        List<Trash> getTrash(int pageNum);
        void TrashRestore(int id);
        void UnlockContent(string objectType, int id);
    }
}
