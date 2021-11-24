package handlers

import (
	"net/http"
	"server/utils"
)

type UnhandledHandler struct {
}

func (h *UnhandledHandler) CanProcess(request *http.Request) bool {
	return true
}

func (h *UnhandledHandler) Process(res http.ResponseWriter, request *http.Request) {
	utils.JsRedirectWithMessage(res,
		"<h1>Hummmmm...</h1> </br> Redirecting in 3 seconds")
}
