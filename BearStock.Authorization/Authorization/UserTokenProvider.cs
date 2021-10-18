using System;
using System.Net.Http;
using System.Threading.Tasks;
using BearStock.Authorization.Helpers;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace BearStock.Authorization.Authorization
{
    public class TokenData
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string Scope { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UserTokenProvider
    {
        private readonly ClientTokenSettings _settings;

        public UserTokenProvider(IOptions<ClientTokenSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<TokenData> Get(string username, string password)
        {
            var client = new HttpClient();
            var documentResponse = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                Address = _settings.Authority, Policy = new DiscoveryPolicy {RequireHttps = false}
            });

            if (documentResponse.IsError) throw new Exception(documentResponse.Error);


            // Grab a bearer token using resource owner password
            var tokenClient = new TokenClient(client,
                new TokenClientOptions {
                    Address = documentResponse.TokenEndpoint,
                    ClientId = _settings.ClientId,
                    ClientSecret = _settings.ClientSecret
                }
            );

            var tokenResponse = await tokenClient.RequestPasswordTokenAsync(username, password, _settings.Scope);

            if (tokenResponse.IsError) throw new Exception(tokenResponse.Error);

            return new TokenData {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                Scope = tokenResponse.Scope
            };
            ;
        }
    }
}