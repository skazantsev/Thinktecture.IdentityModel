using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Thinktecture.IdentityModel.Hawk.Core.Helpers;
using Thinktecture.IdentityModel.Hawk.Core.MessageContracts;
using Thinktecture.IdentityModel.Hawk.Core.Extensions;
using Thinktecture.IdentityModel.Hawk.Etw;

namespace Thinktecture.IdentityModel.Hawk.Core
{
    /// <summary>
    /// Hawk authentication options.
    /// </summary>
    public class Options
    {
        public Options()
        {
            this.ClockSkewSeconds = 60;
            this.EnableServerAuthorization = true;
            this.DetermineHostDetailsCallback = DefaultBehavior.DetermineHostDetails;
        }

        /// <summary>
        /// Local time offset in milliseconds.
        /// </summary>
        public int LocalTimeOffsetMillis { get; set; }

        /// <summary>
        /// Skew allowed between the client and the server clocks in seconds. Default is 60 seconds.
        /// </summary>
        public int ClockSkewSeconds { get; set; }

        /// <summary>
        /// If true, the Server-Authorization header is sent in the response. Default is true.
        /// </summary>
        public bool EnableServerAuthorization { get; set; }

        /// <summary>
        /// Func delegate that returns Credential for the given user identifier.
        /// </summary>
        public Func<string, Credential> CredentialsCallback { get; set; }

        /// <summary>
        /// Func delegate that returns the normalized form of the response message to be used
        /// as application specific data ('ext' field) in the Server-Authorization response header.
        /// </summary>
        public Func<IResponseMessage, string> NormalizationCallback { get; set; }

        /// <summary>
        /// Func delegate that returns true if the specified normalized form of the request
        /// message matches the normalized form of the specified request message.
        /// </summary>
        public Func<IRequestMessage, string, bool> VerificationCallback { get; set; }

        /// <summary>
        /// Func delegate that returns true, if the response body must be hashed and included
        /// in the MAC ('mac' field) sent in the Server-Authorization response header.
        /// </summary>
        public Func<IRequestMessage, bool> ResponsePayloadHashabilityCallback { get; set; }

        /// <summary>
        /// Func delegate that returns the host name and port number.
        /// </summary>
        public Func<IRequestMessage, Tuple<string, string>> DetermineHostDetailsCallback { get; set; }

        public class DefaultBehavior
        {
            internal static Tuple<string, string> DetermineHostDetails(IRequestMessage request)
            {
                string host = request.Headers.FirstOrDefault("X-Forwarded-Host");

                HawkEventSource.Log.Debug("X-Forwarded-Host=" + (host ?? String.Empty));

                if (String.IsNullOrWhiteSpace(host))
                    host = request.Host;

                HawkEventSource.Log.Debug("Host=" + (host ?? String.Empty));

                if (String.IsNullOrWhiteSpace(host))
                    host = request.Uri.Host;

                string hostName = String.Empty;
                string port = String.Empty;

                string pattern = @"^(?:(?:\r\n)?\s)*((?:[^:]+)|(?:\[[^\]]+\]))(?::(\d+))?(?:(?:\r\n)?\s)*$";
                var match = Regex.Match(host, pattern);

                if (match.Success && match.Groups.Count == 3)
                {
                    hostName = match.Groups[1].Value;

                    if (!String.IsNullOrWhiteSpace(hostName))
                    {
                        port = match.Groups[2].Value;
                    }
                }

                if (String.IsNullOrWhiteSpace(port))
                {
                    port = request.Headers.FirstOrDefault("X-Forwarded-Port");

                    HawkEventSource.Log.Debug("X-Forwarded-Port=" + (port ?? String.Empty));
                }

                if (String.IsNullOrWhiteSpace(port))
                {
                    string scheme = request.Headers.FirstOrDefault("X-Forwarded-Proto");

                    HawkEventSource.Log.Debug("X-Forwarded-Proto=" + (scheme ?? String.Empty));

                    if (String.IsNullOrWhiteSpace(scheme))
                    {
                        scheme = request.Scheme;
                    }

                    port = "https".Equals(scheme, StringComparison.OrdinalIgnoreCase) ? "443" : "80";

                    HawkEventSource.Log.Debug("Port chosen based on HTTP scheme: " + port);
                }

                return new Tuple<string, string>(hostName, port);
            }
        }
    }
}
