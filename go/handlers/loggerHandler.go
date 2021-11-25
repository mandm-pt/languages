package handlers

import (
	"fmt"
	"net/http"
)

type LoggerHandler struct {
	NextHandler *RequestHandler
}

func (h *LoggerHandler) CanProcess(request *http.Request) bool {
	return true
}

func (h *LoggerHandler) Process(res http.ResponseWriter, request *http.Request) {
	fmt.Printf("Processing module %T\n", *h.NextHandler)

	(*h.NextHandler).Process(res, request)
}
