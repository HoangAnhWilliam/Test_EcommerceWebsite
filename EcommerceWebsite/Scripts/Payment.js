document.addEventListener("DOMContentLoaded", function () {

    const payMethod1 = document.getElementById("pay_method_1");
    const payMethod2 = document.getElementById("pay_method_2");
    const bankAccountNumber = document.getElementById("bank_account_number");


    payMethod1.addEventListener("change", function () {
        if (payMethod1.checked) {

            bankAccountNumber.style.display = "none";
            showQRCode();
        }
    });

    payMethod2.addEventListener("change", function () {
        if (payMethod2.checked) {

            bankAccountNumber.style.display = "block";
            hideQRCode();
        }
    });


    function showQRCode() {

        const qrCodeImage = document.getElementById("qr_code_image");
        qrCodeImage.style.display = "block";
    }


    function hideQRCode() {

        const qrCodeImage = document.getElementById("qr_code_image");
        qrCodeImage.style.display = "none";
    }


    const submitButton = document.querySelector(".js-send-cart");
    submitButton.addEventListener("click", function () {

        const paymentMethod = payMethod1.checked ? "Ví điện tử" : "Sử dụng số tài khoản";
        let accountNumber = "";


        if (payMethod2.checked) {
            accountNumber = bankAccountNumber.value;


            if (accountNumber.length != 11) {
                alert("Vui lòng nhập đúng 11 số tài khoản.");
                return;
            }
        }


    });
});