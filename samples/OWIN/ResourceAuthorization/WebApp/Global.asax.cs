using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization.Mvc;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            GlobalFilters.Filters.Add(new ForbiddenFilterAttribute());
        }
    }
}
