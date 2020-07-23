using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using languages.HttpModules;

namespace languages
{
    class Program
    {
        static IHttpModule[] modules = new IHttpModule[] {
            new FilesModule(),
            new FibonacciModule(),
            new CountryCapitalSearchModule()
        };

        static async Task Main(string[] args)
        {
            if (!HttpListener.IsSupported) Console.WriteLine("HttpListener not supported!");

            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:8080/");

            listener.Start();

            while (true)
            {
                HttpListenerContext context = null;
                try
                {
                    context = listener.GetContext();

                    var module = modules.FirstOrDefault(m => m.CanProcess(context.Request)) ?? new UnhandledModule();
                    await module.Process(context.Request, context.Response);

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

                    string responsePayload = string.Format(HttpUtils.ResponseWithScriptTemplate,
                        HttpUtils.redirectTimerScript,
                        $@"<h1>Error!!!!</h1>
                        </br>
                        {appEx.Message}
                        </br>
                        Redirecting in 3 seconds");

                    var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
                    await context.Response.OutputStream.WriteAsync(responseBytes);
                    context.Response.OutputStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");

                    if (context == null)
                    {
                        continue;
                    }

                    context.Response.OutputStream.Close();

                }
            }
        }
    }
}