using StocksAPI.Data;

namespace StocksAPI.Data
{
    public class StocksService
    {
        private readonly StocksIDMapping _mapping;
        private readonly PolygonIOIntegration _dataRetriever;
        private readonly Dictionary<string, StockInfo> _stocks;

        public StocksService(StocksIDMapping mapping, PolygonIOIntegration dataRetriever) 
        {
            _mapping = mapping;
            _dataRetriever = dataRetriever;

            InitializeStocks();
        }

        private void InitializeStocks()
        {
            var allStocks = _mapping.GetAllStockSymbols();

            _dataRetriever.GetTickersData(allStocks);
        }
    }
}
