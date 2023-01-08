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

        public FavoritesController(FavoritesService stocksService)
        {
            _stocksService = stocksService;
        }

        [Authorize]
        [HttpGet("favorites")]
        public IActionResult GetFavorites()
        {
            var userID = HttpContext.User.Claims.First(claim => claim.Type == "UserID").Value;
            var favorites = _stocksService.GetFavorites(int.Parse(userID));

            return Ok(favorites);
        }

        [Authorize]
        [HttpPost("add-favorite")]
        public IActionResult AddFavorite(int stockID)
        {
            var userID = HttpContext.User.Claims.First(claim => claim.Type == "UserID").Value;
            var addedID = _stocksService.AddFavorite(stockID, int.Parse(userID));

            return Ok(new { ID  = addedID });
        }

        [Authorize]
        [HttpPost("remove-favorite")]
        public IActionResult RemoveFavorite(int stockID)
        {
            var userID = HttpContext.User.Claims.First(claim => claim.Type == "UserID").Value;
            _stocksService.RemoveFavorite(stockID, int.Parse(userID));

            return Ok();
        }
    }
}