using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal class CountryCapitalSearchModule : BaseModule
    {
        const string Path = "/Capital";
        const string paramName = "country";

        protected override string ModuleName => nameof(CountryCapitalSearchModule);

        public override bool CanProcess(HttpListenerRequest request)
        {
            return request.Url.PathAndQuery.StartsWith(Path, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override async Task ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (string.IsNullOrWhiteSpace(request.QueryString.Get(paramName)))
            {
                await PrintUsage(response);
                return;
            }

            string country = request.QueryString.GetValues(paramName)[0];

            using (var client = new HttpClient())
            {
                string webResponse = await client.GetStringAsync($"https://restcountries.eu/rest/v2/name/{country}");
                
                var jsonDocument = JsonDocument.Parse(webResponse);
                var countryInfo = jsonDocument.RootElement.EnumerateArray().FirstOrDefault();
                string capital = countryInfo.GetProperty("capital").GetString();

                string responsePayload = string.Format(HttpUtils.ResponseTemplate, 
                    $"<h1>Results</h1>We think that the capital of <b>{country}</b> is <b>{capital}</b>");

                var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
                await response.OutputStream.WriteAsync(responseBytes);
            }
        }

        private async Task PrintUsage(HttpListenerResponse response)
        {
            string responsePayload = string.Format(HttpUtils.ResponseTemplate,
                $"<h1>Parameter missing</h1><h2>Usage:</h2>{Path}?{paramName}=Portugal");

            var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
            await response.OutputStream.WriteAsync(responseBytes);
        }
    }
}