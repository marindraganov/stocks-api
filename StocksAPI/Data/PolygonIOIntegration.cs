
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace StocksAPI.Data
{
    public class PolygonIOIntegration
    {
        private HttpClient _httpClient;
        private string _apiKey;

        public PolygonIOIntegration(IConfiguration config)
        {
            _apiKey = config["poligonKey"];

            _httpClient = new()
            {
                BaseAddress = new Uri("https://api.polygon.io"),
            };
        }

        //Ticker Details V3
        // AllSharesCount = weighted_shares_outstanding
        // Company name = name
        public async Task<IEnumerable<TickerDetails>> GetTickersDetails(IEnumerable<string> symbols)
        {
            var result = new List<TickerDetails>();
            foreach (var sym in symbols)
            {
                var requsetParams = new Dictionary<string, string>()
                {
                    { "apiKey", _apiKey }
                };

                using HttpResponseMessage response = await _httpClient.GetAsync(GetQueryURL($"v3/reference/tickers/{sym}", requsetParams));

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<TickerDetails>(jsonResponse);

                result.Add(data);
            }

            for (int i = 0; i < result.Count; i++)
            {
                var det = result[i].results;
                Debug.WriteLine($"\"{i + 1}--{det.Symbol}--{det.CompanyName}--{det.SharesCount}\",");
            }

            return result;
        }


        //Market Status
        //"market": "closed"

        //All Tickers
        // Symbol = ticker
        // Price = d.l
        // Volume = d.v
        public async Task<IEnumerable<TickerInfo>> GetTickersData(IEnumerable<string> symbols)
        {
            var requsetParams = new Dictionary<string, string>()
            {
                { "tickers", string.Join(",", symbols)},
                { "apiKey", _apiKey }
            };

            using HttpResponseMessage response = await _httpClient.GetAsync(GetQueryURL("v2/snapshot/locale/us/markets/stocks/tickers", requsetParams));

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<AllTickersResponse>(jsonResponse);

            var result = data.tickers.Select(t => new TickerInfo
            {
                Symbol= t.Symbol,
                Price = t.day.Price != 0 ? t.day.Price : t.prevDay.Price,
                Volume = t.day.Volume != 0 ? (long)t.day.Volume : (long)t.prevDay.Volume,
                PriceTodayChange = t.PriceTodayChange
            });

            return result;
        }

        private string GetQueryURL(string url, Dictionary<string, string> query)
        {
            string result = url + "?";

            foreach (var item in query)
            {
                result += (string.Format("{0}={1}&", item.Key, item.Value));
            }

            return result;
        }
    }

    public record AllTickersResponse(List<TickerData> tickers);

    public record TickerData(DayData day, DayData prevDay)
    {
        [JsonPropertyName("ticker")]
        public string Symbol { get; set; }

        [JsonPropertyName("todaysChange")]
        public double PriceTodayChange { get; set; }
    }

    public record DayData()
    {
        [JsonPropertyName("c")]
        public double Price { get; set; }

        [JsonPropertyName("v")]
        public double Volume { get; set; }
    }

    public record TickerDetails(TickerDetailsResults results);

    public record TickerDetailsResults()
    {
        [JsonPropertyName("ticker")]
        public string Symbol { get; set; }

        [JsonPropertyName("weighted_shares_outstanding")]
        public long SharesCount { get; set; }

        [JsonPropertyName("name")]
        public string CompanyName { get; set; }
    }
}
