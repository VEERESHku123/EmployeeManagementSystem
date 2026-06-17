async function approveLeave(leaveRequestId) {

    const payload = {
        leaveRequestId: leaveRequestId,
        status: "Approved",
        managerRemark: ""
    };

    await processLeaveRequest(payload);
}

async function rejectLeave(leaveRequestId) {

    const { value: remarks } = await Swal.fire({
        title: 'Reject Leave Request',
        input: 'textarea',
        inputLabel: 'Remarks',
        inputPlaceholder: 'Enter rejection remarks...',
        showCancelButton: true,
        confirmButtonText: 'Reject',
        cancelButtonText: 'Cancel'
    });

    if (!remarks || !remarks.trim()) {
        return;
    }

    const payload = {
        leaveRequestId: leaveRequestId,
        status: "Rejected",
        managerRemark: remarks.trim()
    };

    await processLeaveRequest(payload);
}

async function processLeaveRequest(payload) {

    try {

        const response = await fetch('/manager/approveOrRejectLeave', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        if (result.success) {

            const card = document.querySelector(
                `[data-leave-id="${payload.leaveRequestId}"]`
            );

            if (card) {
                card.remove();
            }

            showAlert(
                'success',
                'Success',
                result.message || 'Leave request updated successfully.'
            );

        } else {

            showAlert(
                'error',
                'Failed',
                result.message || 'Unable to process leave request.'
            );
        }

    } catch (error) {

        console.error(error);

        showAlert(
            'error',
            'Error',
            'Something went wrong while processing the request.'
        );
    }
}