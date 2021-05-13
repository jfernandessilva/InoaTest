using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net.Http;

namespace RestApiClient.Classes
{
    public sealed class ClientApi
    {

       public decimal getPriceFromApi(string _nmAtivo)
        {
            decimal _returnValue;
            string _baseUrlQuotesApi = ConfigurationManager.AppSettings["baseUrlQuotesApi"];
            string _hgApiKey = ConfigurationManager.AppSettings["hgBrasilApiKey"];
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrlQuotesApi}{_hgApiKey}&symbol={_nmAtivo}"),
            };
            using (var response = client.Send(request))
            {
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStringAsync().Result;
                string price = JObject.Parse(body)["results"][_nmAtivo.ToUpperInvariant()]["price"].ToString();
                Decimal.TryParse(price, out _returnValue);
            }

            return
                _returnValue;
        }
    }
}
