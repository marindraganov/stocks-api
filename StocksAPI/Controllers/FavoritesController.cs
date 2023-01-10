using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksAPI.Data;

namespace StocksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoritesController : ControllerBase
    {
        private FavoritesService _stocksService;
        private UserService _userService;

        public FavoritesController(FavoritesService stocksService, UserService userService)
        {
            _stocksService = stocksService;
            _userService = userService;
        }


        [HttpGet("favorites")]
        public IActionResult GetFavorites([FromHeader(Name = "AuthToken")] string token)
        {
            if (!_userService.IsLogged(token)) return Unauthorized();

            var userID = _userService.GetUserID(token);

            var favorites = _stocksService.GetFavorites(userID);

            return Ok(favorites);
        }

        [HttpPost("add-favorite")]
        public IActionResult AddFavorite([FromHeader(Name = "AuthToken")] string token, int stockID)
        {
            if (!_userService.IsLogged(token)) return Unauthorized();

            var userID = _userService.GetUserID(token);

            var addedID = _stocksService.AddFavorite(stockID, userID);

            return Ok(new { ID  = addedID });
        }

        [HttpPost("remove-favorite")]
        public IActionResult RemoveFavorite([FromHeader(Name = "AuthToken")] string token, int stockID)
        {
            if (!_userService.IsLogged(token)) return Unauthorized();

            var userID = _userService.GetUserID(token);

            _stocksService.RemoveFavorite(stockID, userID);

            return Ok();
        }
    }
}