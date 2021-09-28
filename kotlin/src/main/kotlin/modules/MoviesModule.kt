package modules

import HttpUtils.Companion.getQueryStringParameters
import HttpUtils.Companion.writeResponseText
import com.sun.net.httpserver.HttpExchange
import java.sql.Connection
import java.sql.DriverManager
import java.sql.SQLException


class MoviesModule : BaseModule("MoviesModule") {
    private val path = "/Movies"
    private val paramName = "id"

    override fun canProcess(http: HttpExchange): Boolean = http.requestURI.rawPath.startsWith(path, true)

    override fun processingLogic(http: HttpExchange) {
        val parameters = http.requestURI.getQueryStringParameters()

        if (parameters.isEmpty())
            return http.writeResponseText("<h1>Parameter missing</h1><h2>Usage:</h2>$path?$paramName=1")

        val id = parameters[paramName]?.toIntOrNull() ?: throw Exception("You want explode the server! NOT TODAY!")
        val title = getTitleById(id)

        if (title.isNullOrBlank())
            http.writeResponseText("<h1>There's no movie with id $id</h1>")
        else
            http.writeResponseText("<h1>The movie with id $id is $title</h1>")
    }

    private fun getTitleById(id: Int): String? {
        var title: String? = null

        val connection: Connection? = null
        try {
            val connection = DriverManager.getConnection("jdbc:sqlite:./../movies_sqlite.db")

            val pStatement = connection.prepareStatement("SELECT title FROM movies WHERE id = ?")
            pStatement.setInt(1, id)

            val rs = pStatement.executeQuery()

            if (rs.next()) {
                title = rs.getString(1)
            }
        } finally {
            try {
                connection?.close()
            } catch (sqlEx: SQLException) {
                System.err.println(sqlEx.message)
            }
        }

        return title
    }
}