using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Thinktecture.IdentityModel.Client
{
    public class UserInfoClient
    {
        private readonly HttpClient _client;

        public UserInfoClient(Uri endpoint, string token)
            : this(endpoint, token, new HttpClientHandler())
        { }

        public UserInfoClient(Uri endpoint, string token, HttpClientHandler inneHttpClientHandler)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("token");

            if (inneHttpClientHandler == null)
                throw new ArgumentNullException("inneHttpClientHandler");

            _client = new HttpClient(inneHttpClientHandler)
            {
                BaseAddress = endpoint
            };

            _client.SetBearerToken(token);
        }

        public async Task<IEnumerable<Tuple<string, string>>> GetAsync()
        {
            var response = await _client.GetAsync("");

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            JObject jObject;

            try
            {
                jObject = JObject.Parse(json);
            }
            catch (Exception)
            {
                return null;
            }

            var claims = new List<Tuple<string, string>>();
            foreach (var x in jObject)
            {
                claims.Add(Tuple.Create(x.Key, x.Value.ToString()));
            }

            return claims;
        }
    }
}
