using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BearStock.Tools.Handlers
{
    public class ApiKeyHandler : DelegatingHandler
    {
        private readonly string _escapedKey;
        private string _escapedValue;

        public ApiKeyHandler(string key, string value) 
        {
            // escape the key since it might contain invalid characters
            _escapedKey = Uri.EscapeDataString(key);
            _escapedValue = Uri.EscapeDataString(value);

        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // we'll use the UriBuilder to parse and modify the url
            var uriBuilder = new UriBuilder(request.RequestUri);

            // when the query string is empty, we simply want to set the appid query parameter
            if (string.IsNullOrEmpty(uriBuilder.Query))
            {
                uriBuilder.Query = $"{_escapedKey}={_escapedValue}";
            }
            // otherwise we want to append it
            else
            {
                uriBuilder.Query = $"{uriBuilder.Query}&{_escapedKey}={_escapedValue}";
            }
            // replace the uri in the request object
            request.RequestUri = uriBuilder.Uri;
            // make the request as normal
            return base.SendAsync(request, cancellationToken);
        }
    }
}