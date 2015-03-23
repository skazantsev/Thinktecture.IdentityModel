using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.SystemWeb;
using Thinktecture.IdentityModel.SystemWeb.Mvc;

namespace ClaimsAuthorizeSample.Controllers
{
    public class HomeController : Controller
    {
        [ResourceActionAuthorize("View", "Home")]
        public ActionResult Index()
        {
            return View();
        }

        [ResourceActionAuthorize("View", "About")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [ResourceActionAuthorize("View", "Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}