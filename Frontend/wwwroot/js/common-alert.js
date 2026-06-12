window.showAlert = function (icon, title, text, timer = 4000) {

    Swal.fire({
        icon: icon,
        title: title,
        text: text,
        position: 'top-end',
        toast: true,
        timer: timer,
        timerProgressBar: true,
        showConfirmButton: false,
        width: '320px',
        customClass: {
            popup: 'app-toast',
            title: 'app-toast-title',
            htmlContainer: 'app-toast-text'
        }
    });
};