using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface ITrashRepository
    {
        void Create(Trash m_Trash);
        Trash RetrieveOne(int id);
        List<Trash> RetrieveAll();
        void Restore(int id);
        void Delete(int id);
    }
}
