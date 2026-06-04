function loadEmployee(employeeId) {

    $.ajax({
        url: '/employee',
        type: 'GET',
        data: { employeeId: employeeId },
        success: function (result) {
            $('#mainContent').html(result);
        },
        error: function (xhr) {
            console.log(xhr.responseText);
            alert('Failed to load employee details.');
        }
    });
}