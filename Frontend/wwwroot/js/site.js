// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function loadPage(url) {

    fetch(url)
        .then(response => response.text())
        .then(html => {

            document.getElementById('mainContent').innerHTML = html;

        })
        .catch(error => console.error(error));
}