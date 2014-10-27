using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Hawk.Core;
using Thinktecture.IdentityModel.Hawk.Core.MessageContracts;
using Thinktecture.IdentityModel.Hawk.Owin.Extensions;

namespace Thinktecture.IdentityModel.Hawk.Owin
{
    public class HawkAuthenticationHandler : AuthenticationHandler<HawkAuthenticationOptions>
    {
        private HawkServer server = null;
        private Stream stream = null;
        private MemoryStream requestBuffer = null, responseBuffer = null;

        protected async override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            if (Request.IsPayloadHashPresent())
            {
                // buffer the request body
                requestBuffer = new MemoryStream();
                await Request.Body.CopyToAsync(requestBuffer);
                Request.Body = requestBuffer;
            }

            IRequestMessage requestMessage = new OwinRequestMessage(Request);

            server = new HawkServer(requestMessage, Options.HawkOptions);

            var principal = await server.AuthenticateAsync();

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var callback = Options.HawkOptions.ResponsePayloadHashabilityCallback;
                if (callback != null && callback(requestMessage)) // buffer the response body
                {
                    stream = Response.Body;
                    responseBuffer = new MemoryStream();
                    Response.Body = responseBuffer;
                }

                return new AuthenticationTicket(principal.Identity as ClaimsIdentity, (AuthenticationProperties)null);
            }

            return new AuthenticationTicket(null, (AuthenticationProperties)null);
        }

        protected override async Task TeardownCoreAsync()
        {
            if (responseBuffer != null)
            {
                responseBuffer.Seek(0, SeekOrigin.Begin);
                await responseBuffer.CopyToAsync(stream);
            }
        }

        protected override async Task ApplyResponseChallengeAsync()
        {
            IResponseMessage responseMessage = new OwinResponseMessage(Response);

            var header = await server.CreateServerAuthorizationAsync(responseMessage);

            if (header != null)
                responseMessage.AddHeader(header.Item1, header.Item2);
        }
    }
}
