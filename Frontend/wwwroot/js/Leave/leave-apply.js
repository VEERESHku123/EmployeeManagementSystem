// ==============================
// Leave Validation Functions
// ==============================

function getSelectedLeaveBalance() {

    const leaveType = document.getElementById("leaveType");

    if (!leaveType || leaveType.selectedIndex < 0)
        return 0;

    const selected = leaveType.options[leaveType.selectedIndex];

    return parseInt(
        selected.getAttribute("data-balance") || "0"
    );
}

function calculateLeaveDays() {

    const startDate =
        document.querySelector('[name="ApplyLeave.StartDate"]');

    const endDate =
        document.querySelector('[name="ApplyLeave.EndDate"]');

    if (!startDate?.value || !endDate?.value)
        return 0;

    const start = new Date(startDate.value);
    const end = new Date(endDate.value);

    return Math.floor(
        (end - start) / (1000 * 60 * 60 * 24)
    ) + 1;
}

function validateLeaveBalance() {

    const availableBalance =
        getSelectedLeaveBalance();

    const requestedDays =
        calculateLeaveDays();

    if (requestedDays <= 0)
        return true;

    if (requestedDays > availableBalance) {

        window.showAlert(
            "warning",
            "Insufficient Balance",
            `Requested ${requestedDays} day(s), but only ${availableBalance} day(s) available.`
        );

        return false;
    }

    return true;
}


// ==============================
// Leave Page Events
// ==============================

document.addEventListener("click", async (e) => {

    if (e.target.closest("#btnApplyLeave")) {

        const historySection =
            document.getElementById("historySection");

        const leaveFormSection =
            document.getElementById("leaveFormSection");

        const applyBtn =
            document.getElementById("btnApplyLeave");

        const backBtn =
            document.getElementById("btnBackToHistory");

        if (historySection && leaveFormSection) {

            historySection.style.display = "none";
            leaveFormSection.style.display = "block";
        }

        if (applyBtn) {
            applyBtn.style.display = "none";
        }

        if (backBtn) {
            backBtn.style.display = "inline-flex";
        }

        return;
    }

    if (e.target.closest("#btnBackToHistory")) {

        const historySection =
            document.getElementById("historySection");

        const leaveFormSection =
            document.getElementById("leaveFormSection");

        const applyBtn =
            document.getElementById("btnApplyLeave");

        const backBtn =
            document.getElementById("btnBackToHistory");

        leaveFormSection.style.display = "none";
        historySection.style.display = "block";

        applyBtn.style.display = "inline-flex";
        backBtn.style.display = "none";

        return;
    }
});

// ==============================
// Leave Type / Date Change
// ==============================

document.addEventListener("change", (e) => {

    const leaveType =
        document.getElementById("leaveType");

    if (e.target.closest("#leaveType")) {

        const selected =
            leaveType.options[leaveType.selectedIndex];

        const balance =
            selected.getAttribute("data-balance") || "--";

        const balanceElement =
            document.getElementById("availableBalance");

        if (balanceElement) {
            balanceElement.textContent = balance;
        }
    }

    if (
        e.target.name === "ApplyLeave.StartDate" ||
        e.target.name === "ApplyLeave.EndDate" ||
        e.target.id === "leaveType"
    ) {
        validateLeaveBalance();
    }
});

// ==============================
// Apply Leave Submit
// ==============================

document.addEventListener("submit", async (e) => {

    const form =
        e.target.closest("#leaveApplyForm");

    if (!form) return;

    e.preventDefault();

    if (!validateLeaveBalance()) {
        return;
    }

    document.querySelectorAll("[data-valmsg-for]")
        .forEach(x => x.textContent = "");

    const formError =
        document.getElementById("formError");

    if (formError) {

        formError.style.display = "none";
        formError.textContent = "";
    }

    try {

        const response = await fetch(
            form.action,
            {
                method: "POST",
                body: new FormData(form)
            });

        const result =
            await response.json();

        if (!response.ok) {

            if (result.errors) {

                Object.keys(result.errors)
                    .forEach(key => {

                        const span =
                            document.querySelector(
                                `[data-valmsg-for="${key}"]`
                            );

                        if (span) {
                            span.textContent =
                                result.errors[key][0];
                        }
                    });
            }

            if (result.message && formError) {

                formError.textContent =
                    result.message;

                formError.style.display =
                    "block";
            }

            return;
        }

        Swal.fire({
            icon: "success",
            title: "Success",
            text: result.message || "Leave applied successfully."
        });

        form.reset();

        const balanceElement =
            document.getElementById("availableBalance");

        if (balanceElement) {
            balanceElement.textContent = "--";
        }

    }
    catch (error) {

        console.error(
            "Apply Leave Error:",
            error
        );

        window.showAlert(
            "error",
            "Error",
            "Something went wrong."
        );
    }
});