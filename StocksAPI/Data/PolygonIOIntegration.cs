
using System.Collections.Generic;
using System.Net.Http;
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
        // AllSharesCount = share_class_shares_outstanding
        // Company name = name

        //Market Status
        //"market": "closed"

        //All Tickers
        // Price = d.l
        // Volume = d.v
        public async Task<IEnumerable<DayData>> GetTickersData(IEnumerable<string> symbols)
        {

            var requsetParams = new Dictionary<string, string>()
            {
                { "tickers", symbols.First() },
                { "apiKey", _apiKey }
            };


            using HttpResponseMessage response = await _httpClient.GetAsync(GetQueryURL("v2/snapshot/locale/us/markets/stocks/tickers", requsetParams));

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<AllTickersResponse>(jsonResponse);

            return data.tickers.Select(t => t.day);
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

    public record TickerData(DayData day);

    public record DayData()
    {
        [JsonPropertyName("c")]
        public decimal Price { get; set; }

        [JsonPropertyName("v")]
        public decimal Volume { get; set; }
    }
}
