using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace languages.HttpModules
{
    internal class FilesModule : BaseModule
    {
        const string searchPath = "./../";
        const string filesRegEx = @"^\/[A-Za-z]+\.html$";

        protected override string ModuleName => nameof(FilesModule);

        Regex testRequest = new Regex(filesRegEx, RegexOptions.Compiled );
        public override bool CanProcess(HttpListenerRequest request)
        {
            return testRequest.IsMatch(request.Url.AbsolutePath);
        }

        protected override async Task ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            string fileName = request.Url.AbsolutePath;

            if (!File.Exists(searchPath + fileName))
            {
                response.StatusCode = 404;
                string responsePayload = string.Format(HttpUtils.ResponseTemplate, "Oops! File not found");
                var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
                await response.OutputStream.WriteAsync(responseBytes);
                return;
            }

            var fileContents = File.ReadAllBytes($"{searchPath}{fileName}");
            await response.OutputStream.WriteAsync(fileContents);
        }
    }
}