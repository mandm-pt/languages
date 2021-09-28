import HttpUtils.Companion.jsRedirectWithMessage
import com.sun.net.httpserver.HttpServer
import modules.*
import java.net.InetSocketAddress

fun main() {
    val defaultPort = 8080
    val modules =
        listOf(
            FilesModule(),
            RomanModule(),
            GuestBookModule(),
            CountryCapitalSearchModule(),
            MoviesModule(),
            UnhandledModule()
        )

    HttpServer.create(InetSocketAddress(defaultPort), 0).apply {
        createContext("/") { http ->
            try {
                for (m in modules)
                    if (m.canProcess(http)) {
                        m.process(http)
                        break
                    }
            } catch (ex: Exception) {
                println("ApplicationException: ${ex.message}")

                http.jsRedirectWithMessage(
                    """<h1>Error!!!!</h1>
                    </br>${ex.message}
                    </br>Redirecting in 3 seconds"""
                )
            }
        }
        start()
    }
}