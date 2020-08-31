
function DisappearButton(id, apearId) {
    $('#' + id).css('display', 'none');
    $('#' + apearId).css('display', 'block');

}
function AppearButton(id, apearId) {
    $('#' + id).css('display', 'block');
    $('#' + apearId).css('display', 'none');

}



function loginUser() {
    DisappearButton('btn-login', 'loading-box');
    
    var cellNumber = $('#txtCellNum').val();
   
    if (cellNumber !== '') {
        $.ajax(
            {
                url: "/account/SendOtp",
                data: {
                    cellNumber: cellNumber 
                },
                type: "GET"
            }).done(function (result) {

            if (result === "true") {

                DisappearButton('login-form', 'otp-form');

            } else if (result === "false"){

                $('#error-box-login').css('display', 'block');

                $('#error-box-login').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');

                AppearButton('btn-login', 'loading-box');

            } else if (result === "invalidUser"){
              
                DisappearButton('login-form', 'register-form');
                
            } else if (result === "invalidCellNumber"){

                $('#error-box-login').css('display', 'block');

                $('#error-box-login').html('شماره موبایل وارد شده صحیح نمی باشد ');

                AppearButton('btn-login', 'loading-box');

            }
        });
    } else {
        $('#error-box-login').css('display', 'block');
        $('#error-box-login').html('لطفا شماره موبایل خود را وارد نمایید');
        AppearButton('btn-login', 'loading-box');
    }
}

function CompleteRegisterFrom() {
    DisappearButton('btn-register', 'register-loading-box');
   
    var name = $('#fullName').val();
    
    var cellNumber = $('#txtCellNum').val();
   

    if (name !== '' && cellNumber !== '') {
        $.ajax(
            {
                url: "/account/CompleteRegister",
                data: {
                    fullName: name,
                    cellNumber: cellNumber
                },
                type: "GET"
            }).done(function (result) {
            if (result !== "false") {
                DisappearButton('register-form', 'otp-form');

            } else {
                $('#error-box-register').css('display', 'block');
                $('#error-box-register').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                AppearButton('btn-register', 'register-loading-box');

            }
        });
    } else {
        $('#error-box-register').css('display', 'block');
        $('#error-box-register').html('لطفا اطلاعات بالا را تکمیل کنید');
        AppearButton('btn-register', 'register-loading-box');
    }
}

function checkUserOtp() {
    DisappearButton('btn-checkOtp', 'activation-loading-box');
   
    var cellNumber = $('#txtCellNum').val();
    var code = $('#txtCode').val();

    if (code !== '' ) {
        $.ajax(
            {
                url: "/account/CheckOtp",
                data: {
                    activationCode: code,
                    cellNumber: cellNumber,
                },
                type: "GET"
            }).done(function (result) {
                if (result === "invalid") {
                    $('#error-box-otp').css('display', 'block');
                    $('#error-box-otp').html('کد وارد شده صحیح نمی باشد');
                    AppearButton('btn-checkOtp', 'activation-loading-box');

                }
                else if (result.includes("true")) {
                if (result === "true-admin") {
                    window.location = "/orders";
                  
                } else {
                    window.location = "/orders/list";
                }
            } else {
                $('#error-box-otp').css('display', 'block');
                $('#error-box-otp').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                AppearButton('btn-checkOtp', 'activation-loading-box');

            }
        });
    } else {
        $('#error-box-otp').css('display', 'block');
        $('#error-box-otp').html('کد فعال سازی را وارد کنید');
        AppearButton('btn-checkOtp', 'activation-loading-box');
    }
}