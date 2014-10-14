using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Thinktecture.IdentityModel.Hawk.Core.MessageContracts
{
    /// <summary>
    /// Represents an HTTP Request message applicable to Hawk authentication.
    /// </summary>
    public interface IRequestMessage : IMessage
    {
        /// <summary>
        /// Per-request placeholder for the challenge parameter
        /// </summary>
        string ChallengeParameter { get; set; }

        /// <summary>
        /// Host header value
        /// </summary>
        string Host { get; }

        /// <summary>
        /// X-Forwarded-Host header value
        /// </summary>
        string ForwardedHost { get; }

		/// <summary>
		/// X-Forwarded-Port header value
		/// </summary>
		string ForwardedPort { get; }

		/// <summary>
		/// X-Forwarded-Proto header value
		/// </summary>
		string ForwardedProto { get; }

		/// <summary>
        /// Authorization header value
        /// </summary>
        AuthenticationHeaderValue Authorization { get; set; }

        /// <summary>
        /// Request URI
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Query string setter for setting query string sans bewit.
        /// </summary>
        string QueryString { set; }

        /// <summary>
        /// HTTP Method
        /// </summary>
        HttpMethod Method { get; }
    }
}
