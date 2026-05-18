let debounceTimer;

// SEARCH INPUT
document.getElementById("searchInput")
    .addEventListener("input", function () {

        clearTimeout(debounceTimer);

        debounceTimer = setTimeout(() => {

            loadEmployees(1);

        }, 500);

    });

// PAGE SIZE CHANGE
document.getElementById("pageSizeSelect")
    .addEventListener("change", function () {

        loadEmployees(1);

    });

// COMMON AJAX FUNCTION
function loadEmployees(page) {

    const search = document.getElementById("searchInput").value;
    const pageSize = document.getElementById("pageSizeSelect").value;

    fetch(`/Employee/GetAllEmployees?search=${encodeURIComponent(search)}&page=${page}&pageSize=${pageSize}`, {
        headers: {
            "X-Requested-With": "XMLHttpRequest"
        }
    })
        .then(response => response.text())
        .then(html => {

            document.getElementById("employeeTableContainer").innerHTML = html;

            attachPaginationEvents();

        });

}

// PAGINATION AJAX
function attachPaginationEvents() {

    const links = document.querySelectorAll(".pagination-link");

    links.forEach(link => {
        link.addEventListener("click", function (e) {
            e.preventDefault();

            const url = new URL(this.href);
            const page = parseInt(url.searchParams.get("page"));

            loadEmployees(page);
        });
    });

}

// INITIALIZE PAGINATION
attachPaginationEvents();