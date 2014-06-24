using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace Owin.ResourceAuthorization.Tests
{
    [Trait("ResourceAuthorizeAttribute", "No action")]
    public class WebApiPingTest : WebApiTestBase
    {
        public class WebApiUnderTestController : ApiController
        {
            [HttpGet, Route("api/ping")]
            public async Task<string> Ping()
            {
                return await Task.FromResult("pong");
            }
        }

        [Fact(DisplayName = "WebApi on Owin up and running")]
        public async Task WebApiOnOwinUpAndRuning()
        {
            var response = await Client.GetAsync("http://testserver/api/ping");
            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("\"pong\"", result);
        }
    }
}
