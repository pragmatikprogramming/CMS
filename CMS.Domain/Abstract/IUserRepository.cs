using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IUserRepository
    {
        bool Create(string usernName, string firstName, string lastName, string email, string passWord);
        User RetrieveOne(int m_Uid);
        List<User> RetrieveAll();
        bool Update(User m_User, string oldUserName);
        bool Delete(int m_Uid);
    }
}
