using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Security.Claims;

[assembly: OwinStartup(typeof(ScopeValidation.Startup))]

namespace ScopeValidation
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                var claims = new List<Claim>
                {
                    new Claim("sub", "dom"),
                    new Claim("scope", "read"),
                    new Claim("scope", "search")
                };

                ctx.Authentication.User = 
                    new ClaimsPrincipal(new ClaimsIdentity(claims, "custom"));
                
                await next();
            });

            // read OR write
            app.RequireScopes("read", "write");

            // read AND write
            //app.RequireScopes("read");
            //app.RequireScopes("write");
            

            app.UseWebApi(WebApiConfig.Register());
        }
    }
}