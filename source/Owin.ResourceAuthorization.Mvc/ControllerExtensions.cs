using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization.Mvc;

namespace System.Web.Mvc
{
    public static class ControllerExtensions
    {
        public static HttpStatusCodeResult AccessDenied(this Controller controller)
        {
            return new AccessDeniedResult();
        }
    }
}
