using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace languages
{
    public static class HttpUtils
    {
        internal const string mainPage = "/index.html";

        internal const string ResponseTemplate = "<HTML><BODY>{0}</BODY></HTML>";

        internal const string ResponseWithScriptTemplate = "<HTML>" + ScriptTemplate + "<BODY>{1}</BODY></HTML>";
        internal const string ScriptTemplate = "<script>{0}</script>";

        internal const string redirectTimerScript = "setTimeout(function () {window.location.href = '" + mainPage + "';}, 3000);";

        internal static async Task WriteResponseTextAsync(this HttpListenerResponse response, string text)
        {
            string responsePayload = string.Format(HttpUtils.ResponseTemplate, text);
            var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
            await response.OutputStream.WriteAsync(responseBytes);
        }

        internal static async Task JsRedirectWithMessageAsync(this HttpListenerResponse response, string text)
        {
            string responsePayload = string.Format(HttpUtils.ResponseWithScriptTemplate,
                HttpUtils.redirectTimerScript,
                text);

            var responseBytes = Encoding.UTF8.GetBytes(responsePayload);
            await response.OutputStream.WriteAsync(responseBytes);
        }
    }
}