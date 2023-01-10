using System.Text.Json.Serialization;

namespace StocksAPI.Data
{
    public class StockInfo
    {
        public int ID { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public long SharesCount { get; set; }

        public double Price { get; set; }

        public long Volume { get; set; }

        [JsonIgnore]
        public double TodayPriceChange { get; set; }

        public double TodayPriceChangePercent => (TodayPriceChange / Price)  * 100;

        public long MarketCap => (long)(Price * SharesCount);
    }
}
