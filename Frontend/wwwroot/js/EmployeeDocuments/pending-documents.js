function reviewDocuments(employeeId) {

    const url = `/document/verify?employeeId=${employeeId}`;

    fetch(url)
        .then(response => response.text())
        .then(html => {

            document.getElementById("mainContent").innerHTML = html;

            // Update browser URL
            history.pushState({}, "", url);

        })
        .catch(error => console.error(error));
}