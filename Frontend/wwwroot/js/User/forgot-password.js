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
        showAlert('error', 'Error', 'Please enter email.');
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

                showAlert('success', 'OTP Sent', response.message, 2000);
            }
            else {

                showAlert('error', 'Error', response.message);
            }
        },

        error: function () {

            showAlert(
                'error',
                'Error',
                'Something went wrong. Please try again.'
            );
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

                startOtpTimer(response.data.expiresAt);

                showAlert(
                    'success',
                    'OTP Resent',
                    response.message,
                    2000
                );
            }
            else {

                showAlert(
                    'error',
                    'Error',
                    response.message
                );
            }
        },

        error: function () {

            showAlert(
                'error',
                'Error',
                'Something went wrong. Please try again.'
            );
        }
    });
}

function verifyOtp() {

    const email = $("#email").val();

    let otp = "";

    $(".otp-digit").each(function () {
        otp += $(this).val();
    });

    if (otp.length !== 6) {

        showAlert(
            'error',
            'Error',
            'Please enter the 6-digit OTP.'
        );

        return;
    }

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

                showAlert(
                    'success',
                    'OTP Verified',
                    response.message,
                    2000
                );
            }
            else {

                showAlert(
                    'error',
                    'Error',
                    response.message
                );
            }
        },

        error: function () {

            showAlert(
                'error',
                'Error',
                'Something went wrong. Please try again.'
            );
        }
    });
}

function isStrongPassword(password) {
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$/.test(password);
}

function resetPassword() {

    const newPassword = $("#newPassword").val().trim();
    const confirmPassword = $("#confirmPassword").val().trim();
    const resetToken = $("#resetToken").val();

    if (!isStrongPassword(newPassword)) {

        showAlert(
            'warning',
            'Weak Password',
            'Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character.'
        );

        return;
    }

    if (newPassword !== confirmPassword) {

        showAlert(
            'error',
            'Password Mismatch',
            'Passwords do not match.'
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

                showAlert(
                    'success',
                    'Password Updated',
                    response.message,
                    2000
                );

                window.location.href = '/Home/Index?showLogin=true';
            }
            else {

                showAlert(
                    'error',
                    'Error',
                    response.message
                );
            }
        },

        error: function () {

            showAlert(
                'error',
                'Error',
                'Something went wrong. Please try again.'
            );
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
