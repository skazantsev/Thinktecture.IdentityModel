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
		private const string HTTP_PROTO = "http";
		private const string HTTPS_PROTO = "https";
        private const string MATCH_PATTERN_HOSTNAME_OR_IPV4 = @"^(?:(?:\r\n)?\s)*([^:]+)(?::(\d+))?(?:(?:\r\n)?\s)*$";
        private const string MATCH_PATTERN_IPV6 = @"^(?:(?:\r\n)?\s)*(\[[^\]]+\])(?::(\d+))?(?:(?:\r\n)?\s)*$";

        private readonly ArtifactsContainer artifacts = null;

        private readonly string method = null;
        private readonly string path = null;
        private readonly string hostName = null;
        private readonly string port = null;

        internal NormalizedRequest(IRequestMessage request,
                                        ArtifactsContainer artifacts,
                                            HostNameSource? hostNameSource = null,
												PortSource? portSource = null)
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
				string xForwardedPort = (String.IsNullOrWhiteSpace(request.ForwardedPort)) ? null : request.ForwardedPort;
				string xForwardedHostHeaderPort = null;
				string hostHeaderPort = null;

                if (hostNameSource.HasValue) // Case 2a: NOT bewit and user has specified the host name source
                {
                    switch (hostNameSource.Value)
                    {
                        case HostNameSource.XForwardedHostHeader:
                            this.hostName = this.GetHostName(request.ForwardedHost, out xForwardedHostHeaderPort); break;
                        case HostNameSource.HostHeader:
                            this.hostName = this.GetHostName(request.Host, out hostHeaderPort); break;
                        case HostNameSource.RequestUri:
                            this.hostName = request.Uri.Host; break;
                    }
                }

				if (portSource.HasValue) // Case 2b: NOT bewit and user has specified the port source
				{
					switch (portSource.Value)
					{
						case PortSource.XForwardedPortHeader:
							this.port = xForwardedPort; break;
						case PortSource.XForwardedProtoHeader:
							this.port = GetPortFromProto(request.ForwardedProto); break;
						case PortSource.XForwardedHostHeader:
							this.port = xForwardedHostHeaderPort; break;
						case PortSource.HostHeader:
							this.port = hostHeaderPort; break;
						case PortSource.RequestUri:
							this.port = request.Uri.Port.ToString(); break;
					}
				}

                if (String.IsNullOrWhiteSpace(this.hostName))
                {
                    // Case 3a: NOT bewit and user has specified the host name source but unable to determine host name.
                    // Case 4a: NOT bewit and user has NOT specified the host name source.
                    // For both cases, try X-Forwarded-Host header first, then Host header, and finally request URI.

                    this.hostName = this.GetHostName(request.ForwardedHost, out xForwardedHostHeaderPort) ??
                                        this.GetHostName(request.Host, out hostHeaderPort) ??
                                            request.Uri.Host;
                }

				if (String.IsNullOrWhiteSpace(this.port))
				{
					// Case 3b: NOT bewit and user has specified the port source but unable to determine port.
					// Case 4b: NOT bewit and user has NOT specified the port source.
					// For both cases, try X-Forwarded-Port header first, then port based on X-Forwarded-Proto header,
					// then port from X-Forwarded-Host header, then port from Host header, and finally request URI port.

					this.port = xForwardedPort ?? GetPortFromProto(request.ForwardedProto) ??
									xForwardedHostHeaderPort ?? hostHeaderPort ?? request.Uri.Port.ToString();
				}
            }

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

		private string GetPortFromProto(string proto)
		{
			if (String.IsNullOrEmpty(proto))
			{
				return null;
			}

			switch (proto.ToLower())
			{
				case HTTP_PROTO:
					return HTTP_PORT;
				case HTTPS_PROTO:
					return HTTPS_PORT;
				default:
					return null;
			}
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
