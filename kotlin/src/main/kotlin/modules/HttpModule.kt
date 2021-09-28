package modules

import com.sun.net.httpserver.HttpExchange

interface HttpModule {
    fun canProcess(http: HttpExchange): Boolean
    fun process(http: HttpExchange)
}