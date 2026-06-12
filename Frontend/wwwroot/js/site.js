function loadPage(url) {

    fetch(url)
        .then(response => response.text())
        .then(html => {

            document.getElementById('mainContent').innerHTML = html;

            const form = document.querySelector('#mainContent form');

            if (form && $.validator && $.validator.unobtrusive) {

                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");

                $.validator.unobtrusive.parse(form);
            }

            history.pushState({}, "", url);

        })
        .catch(error => console.error(error));
}