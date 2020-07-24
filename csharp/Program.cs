using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using languages.HttpModules;

namespace languages
{
    class Program
    {
        const int defaultPort = 8080;
        static IHttpModule[] modules = new IHttpModule[] {
            new FilesModule(),
            new RomanModule(),
            new CountryCapitalSearchModule(),
            new GuestBookModule(),
            new MoviesModule(),
        };

        static async Task Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HttpListener not supported!");
                Environment.Exit(-1);
            }

            int port = GetPortFromArgs(args);
            
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://+:{port.ToString()}/");

            listener.Start();

            while (true)
            {
                HttpListenerContext context = null;
                try
                {
                    context = listener.GetContext();

                    var module = modules.FirstOrDefault(m => m.CanProcess(context.Request)) ?? new UnhandledModule();
                    await module.ProcessAsync(context.Request, context.Response);

                    await context.Response.OutputStream.FlushAsync();
                    context.Response.OutputStream.Close();
                }
                catch (ApplicationException appEx)
                {
                    Console.WriteLine($"ApplicationException: {appEx.Message}");
                    if (context == null)
                    {
                        continue;
                    }

                    await context.Response.JsRedirectWithMessageAsync($@"<h1>Error!!!!</h1>
                        </br>
                        {appEx.Message}
                        </br>
                        Redirecting in 3 seconds");

                    context.Response.OutputStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    context?.Response?.OutputStream.Close();
                }
            }
        }

        private static int GetPortFromArgs(string[] args)
        {
            int port;
            
            if (args?.Length != 2 || args[0] != "-p")
                return defaultPort;
            else if (!int.TryParse(args[1], out port))
                return defaultPort;
            
            return port;
        }
    }
}