using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;
using Thinktecture.IdentityModel.WebApi;
using Xunit;

namespace Owin.ResourceAuthorization.Tests
{
    [Trait("Resource Authorization Attribute", "Single action, single resource")]
    public class ResourceAuthorizeAttributeTests : WebApiTestBase
    {
        private ResourceAuthorizationContext _context;
        private HttpResponseMessage _response;

        public class ResourceAuthorizeAttributeTestsController : ApiController 
        {
            [HttpGet, Route("api/protected")]
            [ResourceAuthorize("read", "protected")]
            public async Task<HttpResponseMessage> Protected()
            {
                return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
        }

        public ResourceAuthorizeAttributeTests()
        {
            _context = null;

            CheckAccessDelegate = c =>
            {
                _context = c;

                return Task.FromResult(true);
            };

            _response = Client.GetAsync("/api/protected").Result;

        }

        [Fact(DisplayName = "Response should be success")]
        public async Task CheckResponse()
        {
            _response.IsSuccessStatusCode.Should().BeTrue();   
        }

        [Fact(DisplayName = "Context contains Action")]
        public async Task CheckAction()
        {
            _context.ActionNames().Should().Contain("read");
        }

        [Fact(DisplayName = "Context contains Resource")]
        public async Task CheckResource()
        {
            _context.ResourceNames().Should().Contain("protected");
        }
    }

    public static class ResourceAuthorizationContextExtensions
    {
        public static IEnumerable<string> ClaimNames(this IEnumerable<Claim> claims)
        {
            return claims.Where(c => c.Type == "name").Select(c => c.Value);
        }

        public static IEnumerable<string> ActionNames(this ResourceAuthorizationContext context)
        {
            return context.Action.ClaimNames();
        }

        public static IEnumerable<string> ResourceNames(this ResourceAuthorizationContext context)
        {
            return context.Resource.ClaimNames();
        }
    }
}
