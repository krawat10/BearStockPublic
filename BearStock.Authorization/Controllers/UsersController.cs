using System.Security.Claims;
using System.Threading.Tasks;
using BearStock.Authorization.Authorization;
using BearStock.Authorization.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BearStock.Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserTokenProvider _tokenProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            UserTokenProvider tokenProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                user = new ApplicationUser {
                    UserName = model.Username,
                    Email = model.Email,
                    EmailConfirmed = true // Add Confirmation in the future
                };

                await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddClaimsAsync(user,
                    new[] {new Claim(JwtClaimTypes.Name, model.Username), new Claim(JwtClaimTypes.Email, model.Email)});
            }
            else
                return BadRequest("User exists");

            return Ok();
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.UsernameOrEmail.Contains('@')
                    ? await _userManager.FindByEmailAsync(model.UsernameOrEmail)
                    : await _userManager.FindByNameAsync(model.UsernameOrEmail);

                if (user == null)
                    return BadRequest("User does not exists.");

                var result =
                    await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);

                if (result.Succeeded)
                {
                    var tokenData = await _tokenProvider.Get(user.UserName, model.Password);

                    return Ok(new {
                        UserId = user.Id,
                        Username = user.UserName,
                        user.Email,
                        tokenData.AccessToken,
                        tokenData.RefreshToken,
                        tokenData.ExpiresIn,
                        tokenData.Scope
                    });
                }

                if (result.IsLockedOut)
                    return BadRequest();
                return Unauthorized();
            }

            return BadRequest();
        }
    }
}