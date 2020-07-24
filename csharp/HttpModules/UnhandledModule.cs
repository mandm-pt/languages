using System.Net;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal class UnhandledModule : BaseModule
    {
        protected override string ModuleName => nameof(UnhandledModule);

        public override bool CanProcess(HttpListenerRequest request) => true;

        protected override async Task ProcessRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            await response.JsRedirectWithMessageAsync(
                "<h1>Hummmmm...</h1> </br> Redirecting in 3 seconds"
            );
        }
    }
}