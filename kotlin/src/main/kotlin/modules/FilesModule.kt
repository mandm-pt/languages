package modules

import com.sun.net.httpserver.HttpExchange
import java.io.PrintWriter
import java.nio.file.Path
import kotlin.io.path.notExists
import kotlin.io.path.readBytes

class FilesModule : BaseModule("FilesModule") {
    private val chkName = Regex("^\\/[A-Za-z]+\\.html\$")

    override fun canProcess(http: HttpExchange) = chkName.containsMatchIn(http.requestURI.rawPath)

    override fun processingLogic(http: HttpExchange) {
        val currentDir = System.getProperty("user.dir")

        val filePath = Path.of("$currentDir/..${http.requestURI.path}")

        if (filePath.notExists()) {
            http.sendResponseHeaders(404, 0)
            PrintWriter(http.responseBody).use { out ->
                out.println("Oops! File not found")
            }
            return
        }

        http.sendResponseHeaders(200, 0)
        http.responseBody.use {
            it.write(filePath.readBytes())
        }
    }
}