using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using StocksAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using StocksAPI.Controllers.Models;
using Microsoft.AspNetCore.Http.Headers;

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

            var token = _userService.LoginIn(user.ID);

            return Ok(new
            {
                AuthToken = token,
                User = new UserResponse
                {
                    UserID = user.ID,
                    FirstName = user.FisrtName,
                    LastName = user.LastName,
                    Email = user.Email,
                    AvatarID = user.AvatarID
                }
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "AuthToken")] string token)
        {
            if(!_userService.IsLogged(token)) return Unauthorized();

            _userService.LogOut(token);

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

        [HttpGet("get")]
        public IActionResult Get([FromHeader(Name = "AuthToken")] string token)
        {
            if (!_userService.IsLogged(token)) return Unauthorized();

            var userID = _userService.GetUserID(token);

            var user = _userService.GetUser(userID);

            return Ok(new UserResponse
            {
                UserID = user.ID,
                FirstName = user.FisrtName,
                LastName = user.LastName,
                Email = user.Email,
                AvatarID = user.AvatarID
            });
        }

        [HttpPost("update")]
        public IActionResult Update([FromHeader(Name = "AuthToken")] string token, [FromBody] UserUpdateData updateData)
        {
            if (!_userService.IsLogged(token)) return Unauthorized();

            var userID = _userService.GetUserID(token);

            _userService.EditUser(userID, updateData);

            return Ok();
        }

        [HttpPost("add-transaction")]
        public IActionResult AddTransaction([FromHeader(Name = "AuthToken")] string token, [FromBody] TransactionData transaction)
        {
            if (!_userService.IsLogged(token)) return Unauthorized();

            var userID = _userService.GetUserID(token);

            var addedID = _userService.AddUserTransaction(userID, transaction);

            return Ok(new { ID = addedID });
        }

        [HttpPost("remove-transaction")]
        public IActionResult RemoveTransaction([FromHeader(Name = "AuthToken")] string token, int ID)
        {
            if (!_userService.IsLogged(token)) return Unauthorized();

            var userID = _userService.GetUserID(token);

            _userService.RemoveTransactions(userID, ID);

            return Ok();
        }

        [HttpGet("get-transaction")]
        public IActionResult GetTransactions([FromHeader(Name = "AuthToken")] string token)
        {
            if (!_userService.IsLogged(token)) return Unauthorized();

            var userID = _userService.GetUserID(token);

            var transactions = _userService.GetUserTransactions(userID);

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
