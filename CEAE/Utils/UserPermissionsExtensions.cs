using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CEAE.Managers;

namespace CEAE.Utils
{
    public class UserPermissionExactAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly string _matchingSecurity;

        public UserPermissionExactAttribute(string matchingSecurity)
        {
            _matchingSecurity = matchingSecurity;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!AuthenticationManager.IsUserAuthorized(filterContext.HttpContext.Session, _matchingSecurity))
                filterContext.Result = new HttpUnauthorizedResult();
        }
    }

    public class UserPermissionGreaterOrEqualAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly string _matchingSecurity;

        public UserPermissionGreaterOrEqualAttribute(string matchingSecurity)
        {
            _matchingSecurity = matchingSecurity;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!AuthenticationManager.IsUserAuthorizedGreaterOrEqual(filterContext.HttpContext.Session,
                _matchingSecurity))
                filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}