let otpInterval;

function startOtpTimer(expiresAt) {

    clearInterval(otpInterval);

    function updateTimer() {

        const expiry = new Date(expiresAt).getTime();
        const now = new Date().getTime();

        const remaining = expiry - now;

        if (remaining <= 0) {

            clearInterval(otpInterval);

            $("#otpTimer").text("Expired");

            $("#verifyBtn").prop("disabled", true);

            $("#resendOtpBtn").show();

            return;
        }

        const minutes = Math.floor(remaining / 1000 / 60);
        const seconds = Math.floor((remaining / 1000) % 60);

        $("#otpTimer").text(
            `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`
        );
    }

    updateTimer();

    otpInterval = setInterval(updateTimer, 1000);
}

function sendOtp() {

    const email = $("#email").val();

    if (!email) {
        Swal.fire("Error", "Please enter email.", "error");
        return;
    }

    $.ajax({
        url: '/User/ForgotPassword',
        type: 'POST',
        data: { email: email },

        success: function (response) {

            if (response.success) {

                $("#email").prop("readonly", true);

                $("#sendOtpBtn").hide();

                $("#otpArea").slideDown();

                $(".otp-digit").val("");

                startOtpTimer(response.data.expiresAt);

                Swal.fire({
                    icon: "success",
                    title: "OTP Sent",
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
            }
            else {

                Swal.fire(
                    'Error',
                    response.message,
                    'error'
                );
            }
        }
    });
}

function resendOtp() {

    const email = $("#email").val();

    $.ajax({
        url: '/User/ForgotPassword',
        type: 'POST',
        data: { email: email },

        success: function (response) {

            if (response.success) {

                $(".otp-digit").val("");

                $("#verifyBtn").prop("disabled", false);

                $("#resendOtpBtn").hide();

                $("#sendOtpBtn")
                    .prop("disabled", true)
                    .text("OTP Sent");

                startOtpTimer(response.data.expiresAt);

                Swal.fire({
                    icon: 'success',
                    title: 'OTP Resent',
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        }
    });
}

function verifyOtp() {

    const email = $("#email").val();

    let otp = "";

    $(".otp-digit").each(function () {
        otp += $(this).val();
    });

    $.ajax({
        url: '/User/VerifyOtp',
        type: 'POST',
        data: {
            email: email,
            otp: otp
        },

        success: function (response) {

            if (response.success) {

                clearInterval(otpInterval);

                $("#resetToken").val(response.data);

                $("#formTitle").text("Create New Password");

                $("#verificationSection").hide();

                $("#resetSection").fadeIn();

                Swal.fire({
                    icon: 'success',
                    title: 'OTP Verified',
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
            }
            else {

                Swal.fire(
                    'Error',
                    response.message,
                    'error'
                );
            }
        }
    });
}

function resetPassword() {

    const newPassword = $("#newPassword").val();
    const confirmPassword = $("#confirmPassword").val();
    const resetToken = $("#resetToken").val();

    if (newPassword !== confirmPassword) {

        Swal.fire(
            'Error',
            'Passwords do not match',
            'error'
        );

        return;
    }

    $.ajax({
        url: '/User/ResetPassword',
        type: 'POST',
        data: {
            resetToken: resetToken,
            newPassword: newPassword
        },

        success: function (response) {

            if (response.success) {

                Swal.fire({
                    icon: 'success',
                    title: 'Password Updated',
                    text: response.message
                }).then(() => {

                    loadPartial('/User/SignIn');
                });
            }
            else {

                Swal.fire(
                    'Error',
                    response.message,
                    'error'
                );
            }
        }
    });
}

$(document).on("input", ".otp-digit", function () {

    if ($(this).val().length === 1) {

        $(this).next(".otp-digit").focus();
    }
});

$(document).on("keydown", ".otp-digit", function (e) {

    if (e.key === "Backspace" && $(this).val() === "") {

        $(this).prev(".otp-digit").focus();
    }
});