using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.SystemWeb;
using Thinktecture.IdentityModel.SystemWeb.Mvc;

namespace ClaimsAuthorizeSample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
            // global claims authorize filter
            filters.Add(new ResourceActionAuthorizeAttribute());
        }
    }
}
