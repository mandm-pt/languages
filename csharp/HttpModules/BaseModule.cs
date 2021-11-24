using System;
using System.Net;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal abstract class BaseModule : IHttpModule
    {
        protected abstract string ModuleName { get; }

        public abstract bool CanProcess(HttpListenerRequest request);

        public Task ProcessAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            Console.WriteLine($"Processing module {ModuleName}");
            return ProcessRequestAsync(request, response);
        }

        protected abstract Task ProcessRequestAsync(HttpListenerRequest request, HttpListenerResponse response);
    }
}