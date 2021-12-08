using Flurl;
using Flurl.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnionWireless.Utilities.ModelParser.Api.Domain;

namespace UnionWireless.Utilities.ModelParser.Api.Helper
{
    /// <summary>
    /// Class for authenticating calls through credential utility<br/>
    /// <example>
    /// Suggested usage:<br/>
    /// <br/>
    /// For local development and authenticating against and using services on devcluster, recommendation is that the default value for reverse proxy be set as follows:<br/>
    /// &lt;Parameter Name="ReverseProxy" DefaultValue="<c>https://devcluster1.mtview.union-tel.com:19081</c>" /&gt;<br/>
    /// For deployment, set the reverse proxy env variable as follows in the deployment variables:<br/>
    /// "ReverseProxy" = "<c>https://{Host}:19081</c>"<br/>
    /// <br/>
    /// Use the following in your AutoFac configuration:<br/>
    /// <code>
    /// var reverseProxy = EnvironmentHelper.GetRequiredEnvironmentVariable("ReverseProxy");
    /// reverseProxy = reverseProxy.Replace("{Host}", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
    /// builder.RegisterType&lt;CredentialUtilityHelper&gt;()
    ///     .WithParameter("reverseProxy", reverseProxy)
    ///     .WithParameter("clientName", EnvironmentHelper.GetRequiredEnvironmentVariable("ClientName"))
    ///     .AsSelf()
    ///     .SingleInstance();
    ///
    ///
    /// builder.RegisterBuildCallback(scope =&gt;
    /// {
    ///     FlurlHttp.ConfigureClient(reverseProxy, client =&gt;
    ///     {
    ///         client.BeforeCall(async call =&gt; await scope.Resolve&lt;CredentialUtilityHelper&gt;().FlurlLoginCallback(call));
    ///         // Commenting this for now, leaving it here as a reminder.. It may be needed, not sure yet.
    ///         // settings.Settings.HttpClientFactory = new UntrustedCertClientFactory();
    ///     });
    /// });
    /// </code>
    /// </example>
    /// </summary>
    public class CredentialUtilityHelper
    {
        private readonly string _reverseProxy;
        private readonly string _clientName;

        private const string ApiAuthenticateClient = "api/Authenticate/Client";

        /// <summary>
        /// initialCount: 1 so there is a thread available. maxCount: 1 so that only one thread can get a lock.
        /// </summary>
        private readonly SemaphoreSlim _tokenLock = new(1, 1);

        private string _token = "";
        private DateTime _expiration = DateTime.MinValue;

        public CredentialUtilityHelper(ProxySettings reverseProxy, string clientName)
        {
            _reverseProxy = reverseProxy.Address;
            _clientName = clientName;
        }

        private async Task<string> Token()
        {
            await _tokenLock.WaitAsync();
            try
            {
                if (_expiration <= DateTime.Now)
                    (_token, _expiration) = await Login();
            }
            finally
            {
                _tokenLock.Release();
            }

            return _token;
        }

        /// <summary>
        /// Get a login token from the credential utility
        /// </summary>
        private async Task<(string token, DateTime expiration)> Login()
        {
            var authenticateResult = await _reverseProxy
                .AppendPathSegment("UnionWireless.Utilities.Credentials/UnionWireless.Utility.Credential")
                .AppendPathSegment(ApiAuthenticateClient)
                .WithHeader(HeaderNames.Accept, MediaTypeHeaderValue.Parse("application/json"))
                .WithHeader(HeaderNames.ContentType, MediaTypeHeaderValue.Parse("application/json"))
                .PostJsonAsync(new { ClientName = _clientName })
                .ReceiveJson();

            if (authenticateResult?.jsonWebToken == null || !authenticateResult!.success || authenticateResult!.expires <= DateTime.Now)
                throw new Exception("Invalid return for authenticating client");

            return (authenticateResult!.jsonWebToken, authenticateResult.expires);
        }

        /// <summary>
        /// Callback used by flurl. Applied before a call is made and acts to process login if needed.
        /// </summary>
        /// <param name="call"></param>
        public async Task FlurlLoginCallback(FlurlCall call)
        {
            // If the request is not a login request, use the CookieJar property to run a login request if needed and get a fully prepped cookiejar
            if (call.Request.Url.Port == 19081 && !call.Request.Url.Path.Contains(ApiAuthenticateClient) && !call.Request.Headers.Any(header => string.Equals(header.Name, HeaderNames.Authorization, StringComparison.OrdinalIgnoreCase)))
                call.Request.WithOAuthBearerToken(await Token());
        }
    }
}