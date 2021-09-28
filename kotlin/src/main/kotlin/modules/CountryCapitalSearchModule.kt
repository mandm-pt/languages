package modules

import HttpUtils.Companion.getQueryStringParameters
import HttpUtils.Companion.writeResponseText
import com.sun.net.httpserver.HttpExchange
import org.json.JSONArray
import java.net.URI
import java.net.http.HttpClient
import java.net.http.HttpRequest
import java.net.http.HttpResponse


class CountryCapitalSearchModule : BaseModule("CountryCapitalSearchModule") {
    private val path = "/Capital"
    private val paramName = "country"

    override fun canProcess(http: HttpExchange) = http.requestURI.rawPath.equals(path, true)

    override fun processingLogic(http: HttpExchange) {
        val parameters = http.requestURI.getQueryStringParameters()

        if (parameters.isEmpty() && parameters[paramName].isNullOrBlank()) {
            http.writeResponseText("<h1>Parameter missing</h1><h2>Usage:</h2>$path?$paramName=X")
            return
        }

        val country = parameters[paramName] as String

        val capital = callApiAndGetResponse(country)

        http.writeResponseText("<h1>Results</h1>We think that the capital of <b>$country</b> is <b>$capital</b>")
    }

    private fun callApiAndGetResponse(country: String): String {
        val client = HttpClient.newHttpClient()
        val request: HttpRequest? = HttpRequest.newBuilder()
            .uri(URI.create("https://restcountries.com/v2/name/$country"))
            .build()

        var jsonResponse = ""

        client.sendAsync(request, HttpResponse.BodyHandlers.ofString())
            .thenApply { it.body() }
            .thenAccept { jsonResponse = it }
            .join()


        return JSONArray(jsonResponse).getJSONObject(0).getString("capital")
    }
}
