namespace StocksAPI.Data
{
    public class TickerInfo
    {
        public string Symbol { get; set; }

        public double Price { get; set; }

        public long Volume { get; set; }

        public double PriceTodayChange { get; set; }
    }
}
