package main

import (
	"fmt"
	"log"
	"os"
	"server/handlers"
	"server/utils"
	"strconv"

	"net/http"
)

func main() {
	var port int = 8080

	if len(os.Args[1:]) >= 1 {
		userPort, err := strconv.Atoi(os.Args[1])
		if err != nil {
			fmt.Printf("Invalid Port number: %s", os.Args[1])
		}
		port = userPort
	}

	supportedHandlers := []handlers.RequestHandler{
		new(handlers.FileHandler),
		&handlers.GuestBookHandler{}, // different way, same thing
		new(handlers.RomanModule),
		new(handlers.CountryCapitalSearchHandler),
		new(handlers.MoviesHandler),
		new(handlers.AsyncFileHandler),
		new(handlers.UnhandledHandler),
	}

	handler := RoutingHandler{Handlers: supportedHandlers}

	log.Fatal(http.ListenAndServe(fmt.Sprint(":", port), &handler))
}

type RoutingHandler struct {
	Handlers []handlers.RequestHandler
}

func (rh *RoutingHandler) ServeHTTP(res http.ResponseWriter, request *http.Request) {
	defer func(res http.ResponseWriter) {
		if err := recover(); err != nil {
			log.Println("PANIC happened: ", err)

			utils.JsRedirectWithMessage(res, fmt.Sprintf(`<h1>Error!!!!</h1>
				</br>
				%s
				</br>
				Redirecting in 3 seconds`, err))
		}
	}(res)

	for _, h := range rh.Handlers {
		if h.CanProcess(request) {
			hwrapper := new(handlers.LoggerHandler)
			hwrapper.NextHandler = &h
			hwrapper.Process(res, request)
			return
		}
	}
}
