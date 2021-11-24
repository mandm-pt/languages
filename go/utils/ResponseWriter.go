package utils

import (
	"fmt"
	"net/http"
)

func WriteResponseText(res http.ResponseWriter, text string) {
	const responseTemplate = "<HTML><BODY>%s</BODY></HTML>"

	response := fmt.Sprintf(responseTemplate, text)

	res.Write([]byte(response))
}

func JsRedirectWithMessage(res http.ResponseWriter, text string) {
	const mainPage = "/index.html"
	const scriptTemplate = "<script>%s</script>"
	const responseWithScriptTemplate = "<HTML>" + scriptTemplate + "<BODY>%s</BODY></HTML>"
	const redirectTimerScript = "setTimeout(function () {window.location.href = '" + mainPage + "';}, 3000);"

	responsePayload := fmt.Sprintf(responseWithScriptTemplate,
		redirectTimerScript, text)

	var responseBytes = []byte(responsePayload)
	res.Write(responseBytes)
}
