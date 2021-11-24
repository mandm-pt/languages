package handlers

import (
	"io/ioutil"
	"net/http"
	"regexp"
)

type FileHandler struct {
}

func (h *FileHandler) CanProcess(request *http.Request) bool {
	const filesRegEx = "^\\/[A-Za-z]+\\.html$"

	regex, _ := regexp.Compile(filesRegEx)

	return regex.MatchString(request.RequestURI)
}

func (h *FileHandler) Process(res http.ResponseWriter, request *http.Request) {
	const searchPath = "./../"

	requestedFileName := request.URL.Path

	fileBytes, err := ioutil.ReadFile(searchPath + requestedFileName)

	if err != nil {
		http.Error(res, "Oops! File not found", http.StatusNotFound)
		return
	}

	res.Write(fileBytes)
}
