function showCategory(categoryId, button) {
    document.querySelectorAll(".category-section")
        .forEach(section => {
            section.style.display = "none";
        });

    document
        .getElementById("category-" + categoryId)
        .style.display = "block";

    document
        .querySelectorAll(".category-btn")
        .forEach(btn => btn.classList.remove("active"));

    button.classList.add("active");
}

function viewDocument(url) {
    document.getElementById("documentFrame").src = url;

    new bootstrap.Modal(
        document.getElementById("documentModal"))
        .show();
}

function approveDocument(employeeId, documentId) {

    Swal.fire({
        title: 'Approve Document',
        input: 'textarea',
        inputLabel: 'Remarks',
        showCancelButton: true,
        confirmButtonText: 'Approve'
    }).then((result) => {

        if (result.isConfirmed) {

            const form = document.createElement('form');
            form.method = 'post';
            form.action = '/EmployeeDocument/ApproveDocument';

            form.innerHTML = `
                <input type="hidden" name="employeeId" value="${employeeId}" />
                <input type="hidden" name="documentId" value="${documentId}" />
                <input type="hidden" name="remarks" value="${result.value ?? ''}" />
            `;

            document.body.appendChild(form);
            form.submit();
        }
    });
}

function rejectDocument(employeeId, documentId) {

    Swal.fire({
        title: 'Reject Document',
        input: 'textarea',
        inputLabel: 'Remarks',
        inputPlaceholder: 'Enter rejection reason',
        inputValidator: (value) => {
            if (!value) {
                return 'Remarks are required for rejection';
            }
        },
        showCancelButton: true,
        confirmButtonText: 'Reject',
        confirmButtonColor: '#d33'
    }).then((result) => {

        if (result.isConfirmed) {

            const form = document.createElement('form');
            form.method = 'post';
            form.action = '/EmployeeDocument/RejectDocument';

            form.innerHTML = `
                <input type="hidden" name="employeeId" value="${employeeId}" />
                <input type="hidden" name="documentId" value="${documentId}" />
                <input type="hidden" name="remarks" value="${result.value}" />
            `;

            document.body.appendChild(form);
            form.submit();
        }
    });
}