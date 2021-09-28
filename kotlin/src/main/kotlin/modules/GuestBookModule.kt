package modules

import HttpUtils.Companion.writeResponseText
import com.sun.net.httpserver.HttpExchange
import java.net.URLDecoder
import java.nio.charset.StandardCharsets
import java.text.SimpleDateFormat
import java.util.*


class GuestBookModule : BaseModule("GuestBookModule") {
    private val path = "/Guest"
    private val guestsMessages = mutableListOf<String>()

    override fun canProcess(http: HttpExchange) = http.requestURI.rawPath.equals(path, true)

    override fun processingLogic(http: HttpExchange) {
        if (http.requestMethod == "POST") {
            tryAddGuest(http)
        }

        showPage(http)
    }

    private fun tryAddGuest(http: HttpExchange) {
        val text = String(http.requestBody.readAllBytes(), StandardCharsets.ISO_8859_1)

        if (text.startsWith("message=", true)) {
            val message = URLDecoder.decode(text.split("=")[1])
            val currentTime = SimpleDateFormat("dd/MM/yyyy HH:mm:ss").format(Date())

            guestsMessages += "$currentTime - $message"
        }
    }

    private fun showPage(http: HttpExchange) {
        var pageHtml = "<ul>"

        for (message in guestsMessages)
            pageHtml += "<li>$message</li>"

        pageHtml += """</ul>
        <form method='post'>
        <label for='message'>Message</label>
        <input name='message' id='message' autofocus>
        </form>"""

        http.writeResponseText(pageHtml)
    }
}