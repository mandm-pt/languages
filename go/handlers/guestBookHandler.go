package handlers

import (
	"fmt"
	"net/http"
	"server/utils"
	"time"
)

type GuestBookHandler struct {
	guestsMessages []string
}

func (h GuestBookHandler) CanProcess(request *http.Request) bool {
	const path = "/Guest"

	return request.URL.Path == path
}

func (h *GuestBookHandler) Process(res http.ResponseWriter, request *http.Request) {

	if request.Method == "POST" {
		h.tryAddGuestAsync(request)
	}

	h.showPageAsync(res)
}

func (h *GuestBookHandler) tryAddGuestAsync(request *http.Request) {
	msg := request.PostFormValue("message")

	if msg != "" {
		currentTime := time.Now().Format("2006.01.02 15:04:05")
		item := fmt.Sprintf("%s - %s", currentTime, msg)

		h.guestsMessages = append(h.guestsMessages, item)
	}
}

func (h GuestBookHandler) showPageAsync(res http.ResponseWriter) {
	page := "<ul>"

	for _, msg := range h.guestsMessages {
		page += fmt.Sprintf("<li>%s</li>", msg)
	}

	page += `</ul>
	<form method='post'>
		<label for='message'>Message</label>
		<input name='message' id='message' autofocus>
	</form>`

	utils.WriteResponseText(res, page)
}
