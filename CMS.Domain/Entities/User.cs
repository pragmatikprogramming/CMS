using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class User
    {
        private int uid;
        private string userName;
        private string firstName;
        private string lastName;
        private string email;
        private string passWord;

        public int Uid
        {
            get 
            { 
                return uid; 
            }
            set 
            { 
                uid = value; 
            }
        }

        [Required(ErrorMessage = "Please enter you User Name")]
        public string UserName
        {
            get 
            { 
                return userName; 
            }
            set 
            { 
                userName = value; 
            }
        }

        [Required(ErrorMessage = "Please enter your First Name")]
        public string FirstName
        {
            get 
            { 
                return firstName; 
            }
            set 
            { 
                firstName = value; 
            }
        }

        [Required(ErrorMessage = "Please enter your Last Name")]
        public string LastName
        {
            get 
            { 
                return lastName; 
            }
            set 
            { 
                lastName = value; 
            }
        }

        [Required(ErrorMessage = "Please enter an Email Address")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid Email Address")]
        public string Email
        {
            get 
            { 
                return email; 
            }
            set 
            { 
                email = value; 
            }
        }

        [Required(ErrorMessage = "Please enter a Password")]
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        public User()
        {
        }
    }
}