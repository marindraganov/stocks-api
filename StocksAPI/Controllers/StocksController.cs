using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksAPI.Data;

namespace StocksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private StocksService _stocksService;

        public StocksController(StocksService stocksService)
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
            _stocksService.AddFavorite(stockID, int.Parse(userID));

            return Ok();
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