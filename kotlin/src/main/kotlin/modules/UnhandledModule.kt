package modules

import HttpUtils.Companion.jsRedirectWithMessage
import com.sun.net.httpserver.HttpExchange

class UnhandledModule : BaseModule("UnhandledModule") {

    override fun canProcess(http: HttpExchange) = true

    override fun processingLogic(http: HttpExchange) {
        http.jsRedirectWithMessage("<h1>Hummmmm...</h1> </br> Redirecting in 3 seconds")
    }

}