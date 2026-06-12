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

        if (!result.isConfirmed) return;

        fetch('/EmployeeDocument/ApproveDocument', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body:
                `employeeId=${encodeURIComponent(employeeId)}` +
                `&documentId=${encodeURIComponent(documentId)}` +
                `&remarks=${encodeURIComponent(result.value || '')}`
        })
            .then(response => response.text())
            .then(() => {

                showAlert(
                    'success',
                    'Success',
                    'Document approved successfully.'
                );

                setTimeout(() => {
                    loadPage('/document/PendingDocuments');
                }, 1500);
            })
            .catch(error => {

                console.error(error);

                showAlert(
                    'error',
                    'Error',
                    'Failed to approve document.'
                );
            });
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

        if (!result.isConfirmed) return;

        fetch('/EmployeeDocument/RejectDocument', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body:
                `employeeId=${encodeURIComponent(employeeId)}` +
                `&documentId=${encodeURIComponent(documentId)}` +
                `&remarks=${encodeURIComponent(result.value)}`
        })
            .then(response => response.json())
            .then(data => {

                if (data.success) {

                    showAlert(
                        'success',
                        'Rejected',
                        data.message
                    );

                    setTimeout(() => {
                        loadPage('/document/PendingDocuments');
                    }, 1500);
                }
                else {

                    showAlert(
                        'error',
                        'Error',
                        data.message
                    );
                }
            })
            .catch(error => {

                console.error(error);

                showAlert(
                    'error',
                    'Error',
                    'Failed to reject document.'
                );
            });
    });
}