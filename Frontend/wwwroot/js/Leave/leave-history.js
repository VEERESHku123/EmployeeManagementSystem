document.addEventListener("click", async (e) => {

    const tab = e.target.closest(".history-tab");

    if (!tab) return;

    const status = tab.dataset.status;

    document.querySelectorAll(".history-tab")
        .forEach(x => x.classList.remove("active"));

    tab.classList.add("active");

    try {

        const response = await fetch(
            `/Leave/LeaveHistory?status=${encodeURIComponent(status)}`
        );

        if (!response.ok)
            throw new Error();

        const html = await response.text();

        document.getElementById("historyContainer").innerHTML = html;
    }
    catch {

        window.showAlert(
            "error",
            "Error",
            "Failed to load leave history."
        );
    }
});