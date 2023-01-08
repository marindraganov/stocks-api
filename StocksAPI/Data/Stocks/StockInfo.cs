namespace StocksAPI.Data
{
    public class StockInfo
    {
        public int ID { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public long NumberOfShares { get; set; }

        public decimal Price { get; set; }

        public long MarketCap() => (long)(Price * NumberOfShares);
    }
}
