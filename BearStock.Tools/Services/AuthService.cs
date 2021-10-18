using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using BearStock.Tools.Helpers;
using BearStock.Tools.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BearStock.Tools.Services
{
    public class AuthService
    {
        private readonly HttpClient _client;

        public AuthService(IHttpClientFactory clientFactory, IOptions<AuthSettings> appSettings)
        {
            _client = clientFactory.CreateClient();
            _client.BaseAddress = new Uri(appSettings.Value.AuthorizationUrl);
        }

        public async Task<UserDTO> Get(string token)
        {
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");
            var response = await _client.GetAsync("/Users");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDTO>(responseStream);
            }

            return null;
        }
    }

    public class DynamicKeyJwtValidationHandler : JwtSecurityTokenHandler
    {

        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var incomingToken = ReadJwtToken(token);

            validatedToken = incomingToken;

            return new ClaimsPrincipal(new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, "krawat")},
                JwtBearerDefaults.AuthenticationScheme)
            );
        }
    }
}