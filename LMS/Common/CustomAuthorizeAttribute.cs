using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using DAL.DAO;
using DAL.EF;

namespace LMS.Common
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
      
        private readonly string[] allowedRoles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            CommonFunc cFunc = new CommonFunc();
            if (cFunc.GetSession() == null) return false;                    //Chua dang nhap
            else if(cFunc.CheckSessionInvalid() == false) return false;     //Ko dung phien
            else                                                           //check role
            {
                RoleDAO roleDao = new RoleDAO();
                List<ROLE> userRoles = roleDao.GetRoles(cFunc.GetIdUserBySession());

                foreach( var role in allowedRoles)
                {
                    foreach(ROLE uR in userRoles)
                    {
                        if(role == uR.ROLE1)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            CommonFunc cFunc = new CommonFunc();
            if (cFunc.GetSession() == null || cFunc.CheckSessionInvalid() == false)
            {
                filterContext.Result = new RedirectResult("/Home/Index");
                return;
            }
            filterContext.Result = new RedirectResult("/Home/Error");
        }
    }
}