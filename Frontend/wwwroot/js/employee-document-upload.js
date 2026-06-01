function showCategory(categoryId, button) {

    document.querySelectorAll(".category-section")
        .forEach(section => {
            section.style.display = "none";
        });

    document.querySelectorAll(".category-btn")
        .forEach(btn => {
            btn.classList.remove("active");
        });

    document
        .getElementById("category-" + categoryId)
        .style.display = "block";

    button.classList.add("active");
}

function showFileName(input, documentTypeId) {

    if (!input.files.length)
        return;

    const file = input.files[0];

    const fileNameElement =
        document.getElementById(
            "fileName_" + documentTypeId);

    if (fileNameElement) {
        fileNameElement.innerText = file.name;
    }

    const previewFrame =
        document.getElementById(
            "preview_" + documentTypeId);

    if (previewFrame) {

        const url =
            URL.createObjectURL(file);

        previewFrame.src = url;
    }
}

function previewFile(documentTypeId) {

    const frame =
        document.getElementById(
            "preview_" + documentTypeId);

    if (!frame)
        return;

    frame.style.display =
        frame.style.display === "none" ||
            frame.style.display === ""
            ? "block"
            : "none";
}

async function uploadDocument(documentTypeId) {

    const fileInput =
        document.getElementById(
            "newfile_" + documentTypeId);

    if (!fileInput ||
        !fileInput.files.length) {

        Swal.fire({
            icon: "warning",
            title: "No File Selected",
            text: "Please select a file."
        });

        return;
    }

    const result = await Swal.fire({
        title: "Upload Document?",
        text: "Are you sure you want to upload this document?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, Upload",
        cancelButtonText: "Cancel"
    });

    if (!result.isConfirmed)
        return;

    const formData =
        new FormData();

    formData.append(
        "DocumentTypeId",
        documentTypeId);

    formData.append(
        "File",
        fileInput.files[0]);

    try {

        const response =
            await fetch(
                "/EmployeeDocument/UploadDocument",
                {
                    method: "POST",
                    body: formData
                });

        const result =
            await response.json();

        if (result.success) {

            await Swal.fire({
                icon: "success",
                title: "Success",
                text: "Document uploaded successfully."
            });

            location.reload();
        }
        else {

            Swal.fire({
                icon: "error",
                title: "Upload Failed",
                text: result.message || "Upload failed."
            });
        }
    }
    catch (error) {

        console.error(error);

        alert(
            "Upload failed.");
    }
}

function viewDocument(url) {

    document.getElementById(
        "documentFrame").src = url;

    const modal =
        new bootstrap.Modal(
            document.getElementById(
                "documentModal"));

    modal.show();
}

function replaceDocument(employeeId, documentId, documentTypeId) {

    const fileInput =
        document.getElementById(
            "file_" + documentTypeId);

    if (fileInput) {
        fileInput.click();
    }
}

async function deleteDocument(documentId) {

    const confirmed =
        confirm(
            "Are you sure you want to delete this document?");

    if (!confirmed)
        return;

    try {

        const response =
            await fetch(
                `/EmployeeDocument/DeleteDocument?documentId=${documentId}`,
                {
                    method: "DELETE"
                });

        const result =
            await response.json();

        if (result.success) {

            alert(
                "Document deleted successfully.");

            location.reload();
        }
        else {

            alert(
                result.message ||
                "Delete failed.");
        }
    }
    catch (error) {

        console.error(error);

        alert(
            "Delete failed.");
    }
}

async function updateDocument(
    employeeId,
    documentId,
    documentTypeId) {

    const fileInput =
        document.getElementById(
            "file_" + documentTypeId);

    if (!fileInput.files.length)
        return;

    const formData = new FormData();

    formData.append("employeeId", employeeId);
    formData.append("documentId", documentId);
    formData.append("documentTypeId", documentTypeId);
    formData.append("file", fileInput.files[0]);

    const response =
        await fetch(
            "/EmployeeDocument/UpdateDocument",
            {
                method: "POST",
                body: formData
            });

    if (response.redirected) {
        window.location.href = response.url;
    }
    else {
        location.reload();
    }
}
window.addEventListener("load", function () {

    const firstButton =
        document.querySelector(
            ".category-btn");

    if (firstButton) {
        firstButton.click();
    }
});