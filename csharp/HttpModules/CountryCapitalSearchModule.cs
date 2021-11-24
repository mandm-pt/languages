using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal class CountryCapitalSearchModule : BaseModule
    {
        const string Path = "/Capital";
        const string ParamName = "country";

        protected override string ModuleName => nameof(CountryCapitalSearchModule);

        public override bool CanProcess(HttpListenerRequest request)
        {
            return request.Url.PathAndQuery.StartsWith(Path, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override async Task ProcessRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (string.IsNullOrWhiteSpace(request.QueryString.Get(ParamName)))
            {
                await PrintUsageAsync(response);
                return;
            }

            string country = request.QueryString.GetValues(ParamName)[0];

            using (var client = new HttpClient())
            {
                string webResponse = await client.GetStringAsync($"https://restcountries.com/v2/name/{country}");
                
                var jsonDocument = JsonDocument.Parse(webResponse);
                var countryInfo = jsonDocument.RootElement.EnumerateArray().FirstOrDefault();
                string capital = countryInfo.GetProperty("capital").GetString();

                await response.WriteResponseTextAsync(
                    $"<h1>Results</h1>We think that the capital of <b>{country}</b> is <b>{capital}</b>"
                );
            }
        }

        private async Task PrintUsageAsync(HttpListenerResponse response)
        {
            await response.WriteResponseTextAsync(
                $"<h1>Parameter missing</h1><h2>Usage:</h2>{Path}?{ParamName}=Portugal"
            );
        }
    }
}