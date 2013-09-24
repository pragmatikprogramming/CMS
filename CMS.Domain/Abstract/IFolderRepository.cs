using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IFolderRepository
    {
        void Create(Folder m_Folder);
        Folder RetrieveOne(int id);
        List<Folder> RetrieveAll(int id);
        void Update(Folder m_Folder, string OldName);
        void Delete(int id);
    }
}
