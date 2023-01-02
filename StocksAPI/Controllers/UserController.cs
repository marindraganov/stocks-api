using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using StocksAPI.Data;
using StocksAPI.Controllers.Models;
using Microsoft.AspNetCore.Authorization;

namespace StocksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        public UserController(UserService userService) 
        { 
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            var user = _userService.Authenticate(credentials.Password, credentials.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Credentials!");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FisrtName),
                new Claim("UserID", user.ID.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]UserData userData)
        {
            if (_userService.HasUserWithEmail(userData.Email))
            {
                return BadRequest("User Already Exists!");
            }

            _userService.RegisterUser(userData);

            return Ok();
        }

        [Authorize]
        [HttpPost("update")]
        public IActionResult Update([FromBody] UserData userData)
        {
            var userID = HttpContext.User.Claims.First(claim => claim.Type == "UserID").Value;

            _userService.EditUser(int.Parse(userID), userData);

            return Ok();
        }

        [Authorize]
        [HttpGet("hash")]
        public IActionResult GetHash(string pass)
        {
            var t = HttpContext.User.Claims.First(c=> c.Type != "");
            return Ok(_userService.GetHashedPassword(pass));
        }
    }
}
