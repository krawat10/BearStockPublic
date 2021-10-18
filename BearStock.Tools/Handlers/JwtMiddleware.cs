using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BearStock.Tools.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BearStock.Tools.Handlers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthorizationService authorizationService, AuthService authService)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var user = await authService.Get(token);

                if (user != null)
                {
                    context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, user.Username), 
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, "User"), 
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    }));

                    context.Items["userId"] = user.Id;
                }
            }

            await _next(context);
        }
    }
}