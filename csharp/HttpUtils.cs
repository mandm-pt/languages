namespace languages
{
    public static class HttpUtils
    {
        internal const string mainPage = "/index.html";

        internal const string ResponseTemplate = "<HTML><BODY>{0}</BODY></HTML>";

        internal const string ResponseWithScriptTemplate = "<HTML>" + ScriptTemplate + "<BODY>{1}</BODY></HTML>";
        internal const string ScriptTemplate = "<script>{0}</script>";

        internal const string redirectTimerScript = "setTimeout(function () {window.location.href = '" + mainPage + "';}, 3000);";
    }
}