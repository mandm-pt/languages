package handlers

import (
	"fmt"
	"net/http"
	"server/utils"
	"strings"
)

type RomanModule struct {
}

func (h *RomanModule) CanProcess(request *http.Request) bool {
	const path = "/Roman"

	return strings.HasPrefix(request.URL.Path, path)
}

func (h *RomanModule) Process(res http.ResponseWriter, request *http.Request) {
	const paramName = "symbol"
	printUsage := func(res http.ResponseWriter) {
		utils.WriteResponseText(res,
			fmt.Sprintf("<h1>Parameter missing</h1><h2>Usage:</h2>/Roman?%s=X", paramName))
	}

	symbol := request.URL.Query().Get(paramName)
	if symbol == "" {
		printUsage(res)
	}

	var value int

	switch symbol {
	case "I":
		value = 1
	case "V":
		value = 5
	case "X":
		value = 10
	case "L":
		value = 50
	case "C":
		value = 100
	case "D":
		value = 500
	case "M":
		value = 1000
	default:
		panic("You want explode the server! NOT TODAY!")
	}

	utils.WriteResponseText(res,
		fmt.Sprintf("<h1>The value of %s is %d</h1>", symbol, value))
}
