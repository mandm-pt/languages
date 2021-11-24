package handlers

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"server/utils"
	"strings"
)

type CountryCapitalSearchHandler struct {
}

type countryResponse struct {
	Capital string `json:"capital"`
}

func (h *CountryCapitalSearchHandler) CanProcess(request *http.Request) bool {
	const path = "/Capital"

	return strings.HasPrefix(request.URL.Path, path)
}

func (h *CountryCapitalSearchHandler) Process(res http.ResponseWriter, request *http.Request) {
	const paramName = "country"
	printUsage := func(res http.ResponseWriter) {
		utils.WriteResponseText(res,
			fmt.Sprintf("<h1>Parameter missing</h1><h2>Usage:</h2>/Capital?%s=Portugal", paramName))
	}

	countryToGet := request.URL.Query().Get(paramName)

	if countryToGet == "" {
		printUsage(res)
	}

	r, err := http.Get("https://restcountries.com/v2/name/" + countryToGet)
	if err != nil {
		log.Fatalln(err)
	}
	defer r.Body.Close()

	var countries []countryResponse
	err = json.NewDecoder(r.Body).Decode(&countries)

	if err == nil {
		utils.WriteResponseText(res,
			fmt.Sprintf("<h1>Results</h1>We think that the capital of <b>%s</b> is <b>%s</b>",
				countryToGet, countries[0].Capital))
	}
}
