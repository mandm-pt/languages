package modules

import HttpUtils.Companion.getQueryStringParameters
import HttpUtils.Companion.writeResponseText
import com.sun.net.httpserver.HttpExchange

class RomanModule : BaseModule("RomanModule") {
    private val path = "/Roman"
    private val paramName = "symbol"

    override fun canProcess(http: HttpExchange) = http.requestURI.rawPath.startsWith(path, true)

    override fun processingLogic(http: HttpExchange) {
        val parameters = http.requestURI.getQueryStringParameters()

        if (parameters.isEmpty())
            return http.writeResponseText("<h1>Parameter missing</h1><h2>Usage:</h2>$path?$paramName=X")

        val symbol = parameters[paramName]
        val value = when (symbol) {
            "I" -> 1
            "V" -> 5
            "X" -> 10
            "L" -> 50
            "C" -> 100
            "D" -> 500
            "M" -> 1000
            else -> throw Exception("You want explode the server! NOT TODAY!")
        }

        http.writeResponseText("<h1>The value of $symbol is $value</h1>")
    }
}