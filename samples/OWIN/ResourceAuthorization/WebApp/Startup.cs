using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var cookie = new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookie",
                ExpireTimeSpan = TimeSpan.FromMinutes(20),
                LoginPath = new PathString("/Login"),
            };
            app.UseCookieAuthentication(cookie);

            app.UseResourceAuthorization(new ChinookAuthorization());
        }
    }
}