using StocksApp.ServiceContracts;
using System.Text.Json;
namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FinnhubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string StockSympole)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={StockSympole}&token=crjch69r01qo3ctddd8gcrjch69r01qo3ctddd90"),
                    //crjch69r01qo3ctddd8gcrjch69r01qo3ctddd90
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);
                string response = streamReader.ReadToEnd();
                Dictionary<string, object>? responseDictionary =
                JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from finnhub server");
                if(responseDictionary.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                return responseDictionary;
            }
        }
    }
}
