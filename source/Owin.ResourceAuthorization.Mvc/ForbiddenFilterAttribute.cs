using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Thinktecture.IdentityModel.Owin.ResourceAuthorization.Mvc
{
    public class ForbiddenFilterAttribute : ActionFilterAttribute
    {
        string viewName;
        public ForbiddenFilterAttribute()
            : this("AccessDenied")
        {
        }

        public ForbiddenFilterAttribute(string viewName)
        {
            if (String.IsNullOrWhiteSpace(viewName))
            {
                throw new ArgumentNullException("viewName");
            }
            
            this.viewName = viewName;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                var statusCodeResult = filterContext.Result as HttpStatusCodeResult;
                if (statusCodeResult != null &&
                    statusCodeResult.StatusCode == 403)
                {
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = this.viewName
                    };
                }
            }
        }
    }
}
