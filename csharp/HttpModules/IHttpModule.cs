using System.Net;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal interface IHttpModule
    {
        bool CanProcess(HttpListenerRequest request);
        Task ProcessAsync(HttpListenerRequest request, HttpListenerResponse response);
    }
}