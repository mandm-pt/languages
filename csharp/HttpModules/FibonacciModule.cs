using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal class FibonacciModule : BaseModule
    {
        const string Path = "/Fibonacci";
        const string paramName = "number";

        protected override string ModuleName => nameof(FibonacciModule);

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

            int number;
            bool success = int.TryParse(request.QueryString.GetValues(paramName)[0], out number);

            if (!success || number > 20)
                throw new ApplicationException("You want explode the server! NOT TODAY!");

            string responsePayload = string.Format(HttpUtils.ResponseTemplate,
                $"<h1>The Fibonacci of <b>{number}</b> is {Fib(number)}");
                
            var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
            await response.OutputStream.WriteAsync(responseBytes);
        }

        private static int Fib(int n)
        {
            if (n <= 2)
                return 1;
            else
                return Fib(n - 1) + Fib(n - 2);
        }

        private async Task PrintUsage(HttpListenerResponse response)
        {
            string responsePayload = string.Format(HttpUtils.ResponseTemplate,
                $"<h1>Parameter missing</h1><h2>Usage:</h2>{Path}?{paramName}=5");

            var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
            await response.OutputStream.WriteAsync(responseBytes);
        }
    }
}