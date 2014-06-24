using Chinook.Repository;
using Chinook.Repository.InMemory;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        IClaimsRepository claimsRepository;

        public AccountController()
        {
            claimsRepository = new InMemoryClaimsRepository();
        }

        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (model.Username == model.Password)
            {
                var claims = claimsRepository.GetClaimsForUser(model.Username);
                var ci = new ClaimsIdentity(claims, "Cookie");

                var ctx = Request.GetOwinContext();
                ctx.Authentication.SignIn(ci);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        [Route("Logout")]
        public ActionResult Logout()
        {
            if (Request.IsAuthenticated)
            {
                var ctx = Request.GetOwinContext();
                ctx.Authentication.SignOut();
                return RedirectToAction("Logout");
            }

            return View();
        }
    }
}