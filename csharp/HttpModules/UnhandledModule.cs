using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal class UnhandledModule : BaseModule
    {
        protected override string ModuleName => nameof(UnhandledModule);

        public override bool CanProcess(HttpListenerRequest request)
        {
            return true;
        }

        protected override async Task ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responsePayload = string.Format(HttpUtils.ResponseWithScriptTemplate,
                HttpUtils.redirectTimerScript,
                "<h1>Hummmmm...</h1> </br> Redirecting in 3 seconds");

            var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
            await response.OutputStream.WriteAsync(responseBytes);
        }
    }
}