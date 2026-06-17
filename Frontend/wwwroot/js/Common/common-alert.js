window.showAlert = function (icon, title, text, timer = 4000) {

    const requireManualClose =
        icon === "error" || icon === "warning";

    Swal.fire({
        icon,
        title,
        text,

        position: 'top-end',
        toast: !requireManualClose,

        timer: requireManualClose ? undefined : timer,
        timerProgressBar: !requireManualClose,

        showConfirmButton: requireManualClose,
        confirmButtonText: "Close",

        width: '320px',

        customClass: {
            popup: 'app-toast',
            title: 'app-toast-title',
            htmlContainer: 'app-toast-text'
        }
    });
};