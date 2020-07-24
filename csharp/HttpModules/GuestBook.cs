using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal class GuestBookModule : BaseModule
    {
        const string Path = "/Guest";

        private List<string> guestsMessages = new List<string> 
            { $"{DateTime.Now.ToString()} - I was here first!" };
        protected override string ModuleName => nameof(GuestBookModule);

        public override bool CanProcess(HttpListenerRequest request)
        {
            return request.Url.PathAndQuery.Equals(Path, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override async Task ProcessRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.HttpMethod == "POST")
                await TryAddGuestAsync(request);
            
            await ShowPageAsync(response);
        }

        private async Task TryAddGuestAsync(HttpListenerRequest request)
        {
            using var streamReader = new StreamReader(request.InputStream);
            string rawData = await streamReader.ReadToEndAsync();
            
            if (rawData.StartsWith("message="))
            {
                string message = rawData.Split("=")[1];
                guestsMessages.Add($"{DateTime.Now.ToString()} - {message}");
            }
        }

        private async Task ShowPageAsync(HttpListenerResponse response)
        {
            string page = "<ul>";

            foreach (var message in guestsMessages)
            {
                page += $"<li>{message}</li>";
            }

            page += @"</ul>
            <form method='post'>
                <label for='message'>Message</label>
                <input name='message' id='message' autofocus>
            </form>";

            await response.WriteResponseTextAsync(page);
        }
    }
}