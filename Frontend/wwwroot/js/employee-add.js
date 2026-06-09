$(document).on("click", ".mode-btn", function () {

    $(".mode-btn").removeClass("active");
    $(this).addClass("active");

    const isBulkMode = this.id === "bulkModeBtn";

    $("#singleEmployeeSection").toggle(!isBulkMode);
    $("#bulkUploadSection").toggle(isBulkMode);
});

$(document).on("click", "#uploadEmployeesBtn", function () {

    let file = $("#employeeExcel")[0].files[0];

    if (!file) {

        Swal.fire(
            "Error",
            "Please select an Excel file.",
            "error");

        return;
    }

    let formData = new FormData();
    formData.append("file", file);

    $.ajax({
        url: "/employee/upload-employees",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,

        success: function (response) {

            if (response.success) {

                $("#employeeExcel").val("");

                $("#successCount").text(response.data.successCount);
                $("#failedCount").text(response.data.failedCount);

                $("#uploadSummaryContainer").show();

                if (response.data.invalidFileName) {

                    $("#invalidFileDownloadLink").attr(
                        "href",
                        "/employee/download-invalid-file?fileName=" +
                        encodeURIComponent(response.data.invalidFileName));

                    $("#invalidFileContainer").show();
                }
                else {

                    $("#invalidFileContainer").hide();
                }

                if (response.data.invalidEmployeeRecords && response.data.invalidEmployeeRecords.length > 0) {

                    loadInvalidEmployees(response.data.invalidEmployeeRecords);
                }
                else {

                    $("#invalidEmployeesContainer").hide();
                }

                Swal.fire({
                    icon: "success",
                    title: "Upload Completed",
                    html:
                        `Success: ${response.data.successCount}<br>
                         Failed: ${response.data.failedCount}`
                });
            }
            else {

                Swal.fire(
                    "Error",
                    response.message || "Upload failed.",
                    "error");
            }
        },

        error: function () {

            Swal.fire(
                "Error",
                "Something went wrong while uploading the file.",
                "error");
        }
    });
});

function loadInvalidEmployees(records) {

    if ($.fn.DataTable.isDataTable('#errorTable')) {

        $('#errorTable').DataTable().clear().destroy();
    }

    $("#invalidEmployeesTableBody").empty();

    records.forEach(function (item) {

        let employeeName = `${item.employee.firstName} ${item.employee.lastName}`;

        let errors = item.errors.join("<br>");

        $("#invalidEmployeesTableBody").append(`
            <tr>
                <td>${item.employee.employeeId}</td>
                <td>${employeeName}</td>
                <td>${errors}</td>
            </tr>
        `);
    });

    $("#invalidEmployeesContainer").show();

    $('#errorTable').DataTable({
        destroy: true,
        pageLength: 3,
        lengthChange: true,
        lengthMenu: [5, 10, 20, 50, 100],
        ordering: false,
        searching: true,
        info: true,
        retrieve: false,

        language: {
            search: "Search Employee:",
            emptyTable: "No invalid employees found"
        }
    });
}