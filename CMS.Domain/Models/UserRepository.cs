using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class UserRepository : IUserRepository
    {
        public bool Create(string usernName, string firstName, string lastName, string email, string passWord)
        {
            User m_User = new User();
            m_User.UserName = usernName;
            m_User.FirstName = firstName;
            m_User.LastName = firstName;
            m_User.Email = email;
            m_User.PassWord = passWord;

            if (DBUser.isUserNameAvailable(m_User.UserName))
            {
                DBUser.userAdd(m_User);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<User> RetrieveAll()
        {
            List<User> m_Users;
            m_Users = DBUser.GetAll();
            return m_Users;
        }

        public User RetrieveOne(int m_Uid)
        {
            User m_User = DBUser.GetOne(m_Uid);

            return m_User;
        }

        public bool Update(User m_User, string oldUserName)
        {
            if (m_User.UserName != oldUserName)
            {
                if (!DBUser.isUserNameAvailable(m_User.UserName))
                {
                    return false;
                }
            }

            DBUser.userUpdate(m_User);

            return true;
        }

        public bool Delete(int m_Uid)
        {
            DBUser.userDelete(m_Uid);
            return true;
        }
    }
}