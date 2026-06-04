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

            Swal.fire(
                response.success ? "Success" : "Error",
                response.message,
                response.success ? "success" : "error");
        },

        error: function () {

            Swal.fire(
                "Error",
                "Upload failed.",
                "error");
        }
    });
});