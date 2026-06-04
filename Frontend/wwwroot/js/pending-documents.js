function reviewDocuments(employeeId) {

    fetch(`/document/verify?employeeId=${employeeId}`)
        .then(response => response.text())
        .then(html => {

            document.getElementById("mainContent").innerHTML = html;

        })
        .catch(error => console.error(error));
}