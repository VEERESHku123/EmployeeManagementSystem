function loadEmployee(employeeId) {

    $.ajax({
        url: '/employee',
        type: 'GET',
        data: { employeeId: employeeId },
        success: function (result) {

            $('#mainContent').html(result);

            history.pushState(
                {},
                '',
                `/employee?employeeId=${employeeId}`
            );
        },
        error: function (xhr) {
            console.log(xhr.responseText);

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to load employee details.'
            });
        }
    });
}