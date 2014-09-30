using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Mvc
{
    public static class MvcExtensions
    {
        public static HttpStatusCodeResult AccessDenied(this Controller controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");

            if (controller.User != null &&
                controller.User.Identity != null && 
                controller.User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }
    }
}
