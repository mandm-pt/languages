package handlers

import (
	"net/http"
)

type RequestHandler interface {
	CanProcess(req *http.Request) bool
	Process(res http.ResponseWriter, request *http.Request)
}
