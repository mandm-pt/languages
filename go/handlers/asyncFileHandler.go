package handlers

import (
	"fmt"
	"net/http"
	"net/url"
	"sort"
	"strconv"
	"strings"
	"text/template"
	"time"
)

type AsyncFileHandler struct {
	fileProgress map[string]*int
}

func (h *AsyncFileHandler) CanProcess(request *http.Request) bool {
	const path = "/Upload"

	return strings.HasPrefix(request.URL.Path, path)
}

const pageTemplateText string = `<HTML>
	<BODY>
		Auto Refresh:
		<input type="radio" name="AutoRefresh" id="rTrue" value="1" />On
		<input type="radio" name="AutoRefresh" id="rFalse" value="0" />Off
		<br>
		<form id="myForm" method="POST" action="/Upload" onsubmit="DoSubmit();">
			<input type="hidden" name="fileId" id="fileId" />
			<button type="submit">Process new file</button>
		</form>
		<br>
		{{.Progress}}
	</BODY>
	<script language="javascript">
		var autoRefresh = localStorage.getItem('autoRefresh');
		if (autoRefresh == null) localStorage.setItem('autoRefresh', 1);
		
		document.getElementById('rTrue').checked = autoRefresh == 1;
		document.getElementById('rFalse').checked = autoRefresh == 0;

		document.addEventListener('input',(e) =>{
			localStorage.setItem('autoRefresh', e.target.value);
			
			if (e.target.value === true) {
				window.location.reload(1);
			}
		})

		setInterval(function(){
			if (localStorage.getItem('autoRefresh') == 1) {
				window.location.reload(1);
			}
		}, 1000);

		function DoSubmit(){
			document.getElementById('fileId').value = crypto.randomUUID();
			return true;
		}
	</script>
</HTML>
`

func (h *AsyncFileHandler) Process(res http.ResponseWriter, request *http.Request) {
	if request.Method == "POST" && request.ParseForm() == nil {
		h.saveRequest(request.PostForm)

		http.Redirect(res, request, "/Upload", http.StatusSeeOther)
		return
	}

	data := struct {
		Progress string
	}{
		Progress: h.getStatusTable(),
	}

	template, _ := template.New("page").Parse(pageTemplateText)
	template.Execute(res, data)
}

func processFile(fileId string, progress *int) {
	// simulate heavy file processing
	for i := 0; i <= 10; i++ {
		*progress = i * 10
		time.Sleep(1 * time.Second)
	}
}

func (h *AsyncFileHandler) getStatusTable() string {
	table := `<table>
	<tr>
	  <th>File id</th>
	  <th>Progress</th>
	</tr>`

	keys := make([]string, 0, len(h.fileProgress))

	for k := range h.fileProgress {
		keys = append(keys, k)
	}

	sort.Strings(keys)
	for _, k := range keys {
		table += "<tr><td>" + k + "</td><td>" + strconv.Itoa(*h.fileProgress[k]) + "</td></tr>"
	}

	return table + "</table>"
}

func (h *AsyncFileHandler) saveRequest(params url.Values) {
	fmt.Println(params)
	fileId := params.Get("fileId")
	if fileId == "" {
		return
	}

	if h.fileProgress == nil {
		h.fileProgress = make(map[string]*int)
	}

	defaultValue := 0
	h.fileProgress[fileId] = &defaultValue

	go processFile(fileId, h.fileProgress[fileId])
}
