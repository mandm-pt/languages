import com.sun.net.httpserver.HttpExchange
import java.net.URI

class HttpUtils {
    companion object {
        private const val mainPage: String = "/index.html"

        private const val responseTemplate: String = "<HTML><BODY>%s</BODY></HTML>"

        private const val scriptTemplate: String = "<script>%s</script>"
        private const val responseWithScriptTemplate: String = "<HTML>$scriptTemplate<BODY>%s</BODY></HTML>"

        private const val redirectTimerScript: String =
            "setTimeout(function () {window.location.href = '$mainPage';}, 3000);"

        fun HttpExchange.writeResponseText(text: String) {
            val bytesToSend = responseTemplate.format(text).toByteArray()
            this.sendResponseHeaders(200, 0)
            this.responseBody.use {
                it.write(bytesToSend)
            }
        }

        fun HttpExchange.jsRedirectWithMessage(message: String) {
            val bytesToSend = responseWithScriptTemplate.format(redirectTimerScript, message)
                .toByteArray()
            this.sendResponseHeaders(200, 0)
            this.responseBody.use { it.write(bytesToSend) }
        }

        fun URI.getQueryStringParameters(): Map<String, String> {
            return this.query?.split("&")
                ?.map { it.split("=") }
                ?.associate { it.first() to it.last() } ?: emptyMap()
        }
    }
}