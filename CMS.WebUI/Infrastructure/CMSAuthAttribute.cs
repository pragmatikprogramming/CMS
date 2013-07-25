using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;

namespace CMS.WebUI.Infrastructure
{
    public class CMSAuthAttribute : AuthorizeAttribute
    {               
        public CMSAuthAttribute()
        {
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            int uid = (int)(HttpContext.Current.Session["uid"] ?? 0);

            if (uid > 0 && SessionHandler.is_user_authenticated(uid))
            {
                SessionHandler.add_user_session(uid);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}