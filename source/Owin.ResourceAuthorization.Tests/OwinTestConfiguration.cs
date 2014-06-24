using System.Web.Http;

namespace Owin.ResourceAuthorization.Tests
{
    public class OwinTestConfiguration
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}