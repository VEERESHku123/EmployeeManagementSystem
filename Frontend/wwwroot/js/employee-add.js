$(document).on("click", "#singleModeBtn", function () {

    $("#singleEmployeeSection").show();
    $("#bulkUploadSection").hide();

    $(this)
        .removeClass("btn-outline-primary")
        .addClass("btn-primary");

    $("#bulkModeBtn")
        .removeClass("btn-primary")
        .addClass("btn-outline-primary");
});

$(document).on("click", "#bulkModeBtn", function () {

    $("#singleEmployeeSection").hide();
    $("#bulkUploadSection").show();

    $(this)
        .removeClass("btn-outline-primary")
        .addClass("btn-primary");

    $("#singleModeBtn")
        .removeClass("btn-primary")
        .addClass("btn-outline-primary");
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

            $("#uploadErrorsContainer").hide();
            $("#uploadErrorsList").empty();

            if (response.success) {
                console.log(response);
                Swal.fire({
                    icon: "success",
                    title: "Upload Completed",
                    html:
                        `Success: ${response.data.successCount}<br>` +
                        `Failed: ${response.data.failedCount}`
                });

                $("#employeeExcel").val("");

                $("#invalidFileContainer").hide();

                if (response.data &&
                    response.data.invalidFileName) {

                    $("#invalidFileDownloadLink")
                        .attr(
                            "href",
                            "/employee/download-invalid-file?fileName=" +
                            encodeURIComponent(
                                response.data.invalidFileName));

                    $("#invalidFileContainer").show();
                }
            }
            else {

                if (response.errors && response.errors.length > 0) {

                    showUploadErrors(response.errors);
                }

                Swal.fire(
                    "Error",
                    response.message,
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


function showUploadErrors(errors) {

    $("#uploadErrorsList").empty();

    errors.forEach(function (error) {

        $("#uploadErrorsList").append(
            `<li>${error}</li>`
        );
    });

    $("#uploadErrorsContainer").show();
}