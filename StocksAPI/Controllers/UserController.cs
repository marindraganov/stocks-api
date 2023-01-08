using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using StocksAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using StocksAPI.Controllers.Models;

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

            return Ok(new UserLoginResponse{
                UserID = user.ID,
                FirstName = user.FisrtName,
                LastName = user.LastName,
                Email = user.Email,
                AvatarID = user.AvatarID
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]UserRegisterData userData)
        {
            if (_userService.HasUserWithEmail(userData.Email))
            {
                return BadRequest("User Already Exists!");
            }

            var userID = _userService.RegisterUser(userData);

            return Ok(new { UserID = userID });
        }

        [Authorize]
        [HttpPost("update")]
        public IActionResult Update([FromBody] UserUpdateData updateData)
        {
            var userID = HttpContext.User.Claims.First(claim => claim.Type == "UserID").Value;

            _userService.EditUser(int.Parse(userID), updateData);

            return Ok();
        }

        [Authorize]
        [HttpPost("add-transaction")]
        public IActionResult AddTransaction([FromBody] TransactionData transaction)
        {
            var userID = HttpContext.User.Claims.First(claim => claim.Type == "UserID").Value;

            var addedID = _userService.AddUserTransaction(int.Parse(userID), transaction);

            return Ok(new { ID = addedID });
        }

        [Authorize]
        [HttpPost("remove-transaction")]
        public IActionResult RemoveTransaction(int ID)
        {
            var userID = HttpContext.User.Claims.First(claim => claim.Type == "UserID").Value;

            _userService.RemoveTransactions(int.Parse(userID), ID);

            return Ok();
        }

        [Authorize]
        [HttpGet("get-transaction")]
        public IActionResult GetTransactions()
        {
            var userID = HttpContext.User.Claims.First(claim => claim.Type == "UserID").Value;

            var transactions = _userService.GetUserTransactions(int.Parse(userID));

            return Ok(transactions);
        }

        [HttpGet("memory-usage")]
        public IActionResult GetMemoryUsage()
        {
            double memoryBytes = Process.GetCurrentProcess().WorkingSet64;

            return Ok((int)(memoryBytes / (1024*1024)));
        }
    }
}
