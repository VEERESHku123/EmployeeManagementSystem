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

            showAlert(
                'error',
                'Error',
                'Failed to load employee details.'
            );
        }
    });
}