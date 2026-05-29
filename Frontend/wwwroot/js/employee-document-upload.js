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

    document.getElementById(
        "fileName_" + documentTypeId
    ).innerText = file.name;

    const url = URL.createObjectURL(file);

    document.getElementById(
        "preview_" + documentTypeId
    ).src = url;
}

function previewFile(documentTypeId) {

    const frame =
        document.getElementById(
            "preview_" + documentTypeId);

    frame.style.display =
        frame.style.display === "none"
            ? "block"
            : "none";
}

async function uploadDocument(documentTypeId) {

    const fileInput =
        document.getElementById(
            "file_" + documentTypeId);

    if (!fileInput.files.length) {
        alert("Please select a file.");
        return;
    }

    const confirmed =
        confirm(
            "Are you sure you want to upload this document?");

    if (!confirmed)
        return;

    const formData = new FormData();

    formData.append(
        "DocumentTypeId",
        documentTypeId);

    formData.append(
        "File",
        fileInput.files[0]);

    try {

        const response =
            await fetch(
                "/EmployeeDocument/UploadDocuments",
                {
                    method: "POST",
                    body: formData
                });

        const result =
            await response.json();

        if (result.success) {

            document.getElementById(
                "cardBody_" + documentTypeId)
                .style.display = "none";

            document.getElementById(
                "uploaded_" + documentTypeId)
                .style.display = "block";
        }
        else {
            alert(result.message);
        }
    }
    catch {
        alert("Upload failed.");
    }
}

window.addEventListener("load", function () {

    const firstButton =
        document.querySelector(".category-btn");

    if (firstButton) {
        firstButton.click();
    }
});