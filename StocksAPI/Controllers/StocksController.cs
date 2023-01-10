using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksAPI.Data;

namespace StocksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly StocksService _stocksService;

        public StocksController(StocksService stocksService) 
        { 
            _stocksService = stocksService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getCount">max count of returned stocks</param>
        /// <param name="skip">skip number of stocks after filter and order them</param>
        /// <param name="filter">filter phrase should be in Symbol or Name</param>
        /// <param name="orderBy">None = 0, Symbol = 1, Name = 2, Price = 3, MarketCap = 4</param>
        /// <param name="order">Up = 0, Down = 1</param>
        [HttpPost("get-stocks")]
        public IActionResult GetTransactions(int getCount, int skip, string? filter, StocksOrderType orderBy, OrderDirection order)
        {
            var filtered = _stocksService.GetAllStocks()
                .Where(s => Filter(s, filter));

            var result = Order(filtered, orderBy, order)
                .Skip(skip)
                .Take(getCount);

            return Ok(result);
        }

        private IEnumerable<StockInfo> Order(IEnumerable<StockInfo> stocks, StocksOrderType orderBy, OrderDirection order)
        {
            if (orderBy == StocksOrderType.None)
            {
                return stocks.OrderBy(stocks => -stocks.MarketCap);
            }

            var orderedStocks = stocks;
            if (orderBy == StocksOrderType.Symbol)
            {
                orderedStocks = stocks.OrderBy(stocks => stocks.Symbol);
            }
            else if (orderBy == StocksOrderType.Name)
            {
                orderedStocks = stocks.OrderBy(stocks => stocks.Name);
            }
            else if (orderBy == StocksOrderType.Price)
            {
                orderedStocks = stocks.OrderBy(stocks => stocks.Price);
            }
            else if (orderBy == StocksOrderType.MarketCap)
            {
                orderedStocks = stocks.OrderBy(stocks => stocks.MarketCap);
            }

            if (order == OrderDirection.Down)
            {
                orderedStocks = orderedStocks.Reverse();
            }

            return orderedStocks;
        }

        private bool Filter(StockInfo st, string? filter)
        {
            return filter == null || 
                st.Symbol.ToLower().Contains(filter.ToLower()) || 
                st.Name.ToLower().Contains(filter.ToLower());
        }

        public enum StocksOrderType
        {
            None = 0,
            Symbol = 1,
            Name = 2,
            Price = 3,
            MarketCap = 4
        }

        public enum OrderDirection
        {
            Up = 0,
            Down = 1
        }
    }
}
