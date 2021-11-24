package handlers

import (
	"database/sql"
	"fmt"
	"net/http"
	"server/utils"
	"strings"

	_ "github.com/mattn/go-sqlite3"
)

type MoviesHandler struct{}

func (h *MoviesHandler) CanProcess(request *http.Request) bool {
	const path = "/Movies"

	return strings.HasPrefix(request.URL.Path, path)
}

func (h *MoviesHandler) Process(res http.ResponseWriter, request *http.Request) {
	const paramName = "id"
	printUsage := func(res http.ResponseWriter) {
		utils.WriteResponseText(res,
			fmt.Sprintf("<h1>Parameter missing</h1><h2>Usage:</h2>/id?%s=1", paramName))
	}

	idToGet := request.URL.Query().Get(paramName)

	if idToGet == "" {
		printUsage(res)
	}

	db, _ := sql.Open("sqlite3", "./../movies_sqlite.db")

	rows, _ := db.Query(`SELECT title FROM movies WHERE id = ?`, idToGet)

	if !rows.Next() {
		utils.WriteResponseText(res,
			fmt.Sprintf("<h1>There's no movie with id %s</h1>", idToGet))
	} else {
		var title string

		rows.Scan(&title)

		utils.WriteResponseText(res,
			fmt.Sprintf("<h1>The movie with id %s is %s</h1>", idToGet, title))
	}
}
