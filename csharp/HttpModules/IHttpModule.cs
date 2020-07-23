using System.Net;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal interface IHttpModule
    {
        bool CanProcess(HttpListenerRequest request);
        Task Process(HttpListenerRequest request, HttpListenerResponse response);
    }
}