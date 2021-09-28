package modules

import com.sun.net.httpserver.HttpExchange

abstract class BaseModule(private val name: String) : HttpModule {

    override fun process(http: HttpExchange) {
        println("Processing module $name")
        processingLogic(http)
    }

    protected abstract fun processingLogic(http: HttpExchange)
}