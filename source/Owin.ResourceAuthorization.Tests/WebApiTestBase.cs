using System;
using System.Net.Http;
using Microsoft.Owin.Testing;

namespace Owin.ResourceAuthorization.Tests
{
    public class WebApiTestBase : IDisposable
    {
        private readonly TestServer _server;
        
        public WebApiTestBase()
        {
            _server = TestServer.Create<OwinTestConfiguration>();
            Client = new HttpClient(_server.Handler);
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }

        public HttpClient Client { get; private set; }
    }
}