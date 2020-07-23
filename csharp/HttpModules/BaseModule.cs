using System;
using System.Net;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal abstract class BaseModule : IHttpModule
    {
        protected abstract string ModuleName { get; }

        public abstract bool CanProcess(HttpListenerRequest request);

        public Task Process(HttpListenerRequest request, HttpListenerResponse response)
        {
            Console.WriteLine(ModuleName);
            return ProcessRequest(request, response);
        }

        protected abstract Task ProcessRequest(HttpListenerRequest request, HttpListenerResponse response);
    }
}