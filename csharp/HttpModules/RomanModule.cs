using System;
using System.Net;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal class RomanModule : BaseModule
    {
        const string Path = "/Roman";
        const string paramName = "symbol";

        protected override string ModuleName => nameof(RomanModule);

        public override bool CanProcess(HttpListenerRequest request)
        {
            return request.Url.PathAndQuery.StartsWith(Path, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override async Task ProcessRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (string.IsNullOrWhiteSpace(request.QueryString.Get(paramName)))
            {
                await PrintUsageAsync(response);
                return;
            }

            string symbol = request.QueryString.GetValues(paramName)[0];
            int value;

            switch (symbol)
            {
                case "I": value = 1; break;
                case "V": value = 5; break;
                case "X": value = 10; break;
                case "L": value = 50; break;
                case "C": value = 100; break;
                case "D": value = 500; break;
                case "M": value = 1000; break;
                default:
                    throw new ApplicationException("You want explode the server! NOT TODAY!");
            }

            await response.WriteResponseTextAsync(
                $"<h1>The value of {symbol} is {value}</h1>"
            );
        }

        private async Task PrintUsageAsync(HttpListenerResponse response)
        {
            await response.WriteResponseTextAsync(
                $"<h1>Parameter missing</h1><h2>Usage:</h2>{Path}?{paramName}=X"
            );
        }
    }
}