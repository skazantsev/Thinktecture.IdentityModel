using System;
using System.Text;
using System.Text.RegularExpressions;
using Thinktecture.IdentityModel.Hawk.Core.Extensions;
using Thinktecture.IdentityModel.Hawk.Core.Helpers;
using Thinktecture.IdentityModel.Hawk.Core.MessageContracts;

namespace Thinktecture.IdentityModel.Hawk.Core
{
    /// <summary>
    /// Represents the normalized request, in the following format.
    /// hawk.1.header\n
    /// timestamp\n
    /// nonce\n
    /// HTTP method\n
    /// uri path and query string\n
    /// host name\n
    /// port\n
    /// payload hash\n
    /// application specific data\n
    /// </summary>
    internal class NormalizedRequest
    {
        private const string REQUEST_PREAMBLE = HawkConstants.Scheme + "." + HawkConstants.Version + ".header"; // hawk.1.header
        private const string BEWIT_PREAMBLE = HawkConstants.Scheme + "." + HawkConstants.Version + ".bewit"; // hawk.1.bewit
        private const string RESPONSE_PREAMBLE = HawkConstants.Scheme + "." + HawkConstants.Version + ".response"; // hawk.1.response

        private const string HTTP_PORT = "80";
        private const string HTTPS_PORT = "443";
        private const string MATCH_PATTERN_HOSTNAME_OR_IPV4 = @"^(?:(?:\r\n)?\s)*([^:]+)(?::(\d+))?(?:(?:\r\n)?\s)*$";
        private const string MATCH_PATTERN_IPV6 = @"^(?:(?:\r\n)?\s)*(\[[^\]]+\])(?::(\d+))?(?:(?:\r\n)?\s)*$";
        private const string XFF_HEADER_NAME = "X-Forwarded-For";

        private readonly ArtifactsContainer artifacts = null;

        private readonly string method = null;
        private readonly string path = null;
        private readonly string hostName = null;
        private readonly string port = null;

        internal NormalizedRequest(IRequestMessage request,
                                        ArtifactsContainer artifacts,
                                            HostNameSource? hostNameSource = null)
        {
            this.artifacts = artifacts;

            // Case 1: For bewit, host and port are always from the request URI.
            if (IsBewit)
            {
                this.hostName = request.Uri.Host;
                this.port = request.Uri.Port.ToString();
            }
            else
            {
                if (hostNameSource.HasValue) // Case 2: NOT bewit and user has specified the host name source
                {
                    switch (hostNameSource.Value)
                    {
                        case HostNameSource.XForwardedForHeader:
                            this.hostName = this.GetHostName(request.ForwardedFor, out this.port); break;
                        case HostNameSource.HostHeader:
                            this.hostName = this.GetHostName(request.Host, out this.port); break;
                        case HostNameSource.RequestUri:
                                this.hostName = request.Uri.Host; break;
                    }
                }

                if (String.IsNullOrWhiteSpace(this.hostName))
                {
                    // Case 3: NOT bewit and user has specified the host name source but unable to determine host name.
                    // Case 4: NOT bewit and user has NOT specified the host name source.
                    // For both cases, try X-Forwarded-For header first, then Host header, and finally request URI.

                    this.hostName = this.GetHostName(request.ForwardedFor, out this.port) ??
                                        this.GetHostName(request.Host, out this.port) ??
                                            request.Uri.Host;
                }
            }

            if (String.IsNullOrWhiteSpace(this.port))
                this.port = request.Uri.Port.ToString();

            this.method = request.Method.Method.ToUpper();
            this.path = request.Uri.PathAndQuery;
        }

        /// <summary>
        /// Set to true, if this instance is for a bewit.
        /// </summary>
        internal bool IsBewit { get; set; }

        /// <summary>
        /// Set to true, if this instance is for server authorization response.
        /// </summary>
        internal bool IsServerAuthorization { get; set; }

        /// <summary>
        /// Returns the normalized request string.
        /// </summary>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result
                .AppendNewLine(this.GetPreamble())
                .AppendNewLine(artifacts.Timestamp.ToString())
                .AppendNewLine(artifacts.Nonce)
                .AppendNewLine(this.method)
                .AppendNewLine(this.path)
                .AppendNewLine(this.hostName)
                .AppendNewLine(this.port)
                .AppendNewLine(artifacts.PayloadHash == null ? null : artifacts.PayloadHash.ToBase64String())
                .AppendNewLine(artifacts.ApplicationSpecificData);

            return result.ToString();
        }

        /// <summary>
        /// Returns the normalized request bytes.
        /// </summary>
        internal byte[] ToBytes()
        {
            return this.ToString().ToBytesFromUtf8();
        }

        private string GetHostName(string hostHeader, out string port)
        {
            if (!String.IsNullOrWhiteSpace(hostHeader))
            {
                string pattern = hostHeader[0] == '[' ? MATCH_PATTERN_IPV6 : MATCH_PATTERN_HOSTNAME_OR_IPV4;
                var match = Regex.Match(hostHeader, pattern);

                if (match.Success && match.Groups.Count == 3)
                {
                    string hostName = match.Groups[1].Value;

                    if (!String.IsNullOrWhiteSpace(hostName))
                    {
                        port = match.Groups[2].Value;
                        return hostName;
                    }
                }
            }

            port = null;
            return null;
        }

        private string GetPreamble()
        {
            string preamble = REQUEST_PREAMBLE;
            if (this.IsBewit)
                preamble = BEWIT_PREAMBLE;
            else if (this.IsServerAuthorization)
                preamble = RESPONSE_PREAMBLE;

            return preamble;
        }
    }
}
