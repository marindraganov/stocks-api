using StocksAPI.Data;

namespace StocksAPI.Data
{
    public class StocksService
    {
        private readonly StocksBaseInfo _stocksBaseInfo;
        private readonly PolygonIOIntegration _dataRetriever;
        private readonly Dictionary<string, StockInfo> _inmemoryStocks;

        public StocksService(StocksBaseInfo stocksBaseInfo, PolygonIOIntegration dataRetriever) 
        {
            _stocksBaseInfo = stocksBaseInfo;
            _dataRetriever = dataRetriever;
            _inmemoryStocks = new Dictionary<string, StockInfo>();

            InitializeStocks();
        }

        public IEnumerable<StockInfo> GetAllStocks()
        {
            return _inmemoryStocks.Values;
        }

        public IEnumerable<StockInfo> GetStocksByID(IEnumerable<int> stockIDs)
        {
            var result = new List<StockInfo>();

            foreach (var id in stockIDs)
            {
                var symbol = _stocksBaseInfo.GetSymbol(id);
                if (_inmemoryStocks.ContainsKey(symbol))
                {
                    result.Add(_inmemoryStocks[symbol]);
                }
            }

            return result;
        }

        private async void InitializeStocks()
        {
            var allStocks = _stocksBaseInfo.GetAllStocks();

            //_dataRetriever.GetTickersDetails(allStocks.Select(st => st.Symbol));

            await _dataRetriever.GetTickersData(allStocks.Select(st => st.Symbol)).ContinueWith(t =>
            {
                foreach (var ticker in t.Result)
                {
                    var stockBase = _stocksBaseInfo.GetStock(ticker.Symbol);

                    _inmemoryStocks[ticker.Symbol] = new StockInfo
                    {
                        ID = stockBase.ID,
                        SharesCount = stockBase.SharesCount,
                        Name = stockBase.Name,
                        Symbol = ticker.Symbol,
                        Price = ticker.Price,
                        TodayPriceChange = ticker.PriceTodayChange,
                        Volume = ticker.Volume
                    };
                }
            });
        }
    }
}
