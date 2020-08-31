function addDiscountCode() {
    var coupon = $("#coupon").val();
    $('#errorDiv').css('display', 'none');
    if (coupon !== "") {
        $.ajax(
            {
                url: "/Basket/DiscountRequestPost",
                data: { coupon: coupon },
                type: "GET"
            }).done(function (result) {
                if (result !== "Invald" && result !== "Used" && result !== "Expired") {
                    location.reload();
                }
                else if (result !== true) {
                    $('#errorDiv').css('display', 'block');
                    if (result.toLowerCase() === "used") {
                        $('#errorDiv').html("این کد تخفیف قبلا استفاده شده است.");
                    }
                    else if (result.toLowerCase() === "expired") {
                        $('#errorDiv').html("کد تخفیف وارد شده منقضی شده است.");
                    }
                    else if (result.toLowerCase() === "invald") {
                        $('#errorDiv').html("کد تخفیف وارد شده معتبر نمی باشد.");
                    }
                    else if (result.toLowerCase() === "true") {
                        $('#SuccessDiv').css('display', 'block');
                        $('#errorDiv').css('display', 'none');
                    }
                }
            });

    } else {
        $('#SuccessDiv').css('display', 'none');
        $('#errorDiv').html('کد تخفیف را وارد نمایید.');
        $('#errorDiv').css('display', 'block');
    }
}

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function addToBasket(id, duration, productType) {
     
    var cookie = getCookie(productType);
    var newOrderDetails = '';

    if (cookie != null) {
        var orderDetails = cookie.split('/');
        var hasRecord = false;

        for (var i = 0; i < orderDetails.length - 1; i++) {
            var orderDetail = orderDetails[i].split('^');

            if (orderDetail[0] === id) {
                var qty = parseInt(orderDetail[1]) + 1;
                orderDetails[i] = id + "^" + qty;
                hasRecord = true;
            }
            newOrderDetails = newOrderDetails + orderDetails[i] + '/';
        }

        if (hasRecord === false) {
            newOrderDetails = cookie + id + "^1/";
        }
    } else {
        newOrderDetails = id + "^1/";

    }
    deleteCookie(productType);
    setCookie(productType, newOrderDetails, 100);
    
    FillBasket(productType);
}

function FillBasket(productType) {

    //var productTypeCookie = getCookie('productType');

    //if (productTypeCookie !== null && productTypeCookie !== productType) {
    //    deleteCookie('basket');
    //}


    $.ajax({
        url: "/Basket/GetSideBarBasket",
        data: { productType: productType },
        type: "Post",

        success: function (data) {
            console.log(data);
            if (data === "false") {
                alert("خطایی پیش آمده است");
            }
            else {
                if (data.Products.length === 0 || data.Products.length === '') {
                    $(".empty-basket").css('display', 'block');
                    $("#fillBasket").css('display', 'none');
                }
                else {
                    $(".empty-basket").css('display', 'none');
                    $("#fillBasket").css('display', 'block');
                    $(".tbl-basket tbody").html('');
                    for (var i = 0; i < data.Products.length; i++) {
                        $(".tbl-basket tbody").append('<tr>' + '<td>' + data.Products[i].ProductTitle + data.Products[i].Quantity + 'x' + '</td>' + '<td>' + data.Products[i].RowAmount + ' تومان' + '</td><td></td><td><button type="button" onclick="removeFromBasket(\'' + data.Products[i].Id + '\',\'' + productType + '\');">x</button></td></tr>')
                    }
                    $(".tbl-basket tbody").append('<tr style="border-top: 3px solid #006247;">' + '<td><strong>جمع کل</strong></td>' + '<td colspan="2">' + data.SubTotal + '</td></tr>')

                    $(".tbl-basket tbody").append('<tr>' + '<td><strong>تخفیف</strong></td>' + '<td colspan="2">' + data.DiscountAmount + '</td></tr>')

                    $(".tbl-basket tbody").append('<tr>' + '<td><strong>پرداختی</strong></td>' + '<td colspan="2">' + data.Total + ' تومان' + '</td></tr>')

                    $(".tbl-basket tbody").append('<tr>' + '<td colspan="3" style="text-align: center;background: #5cb85c;border-radius: 5px;"><a href="/basket?basketType=' + productType +'"style="color: #fff;">نهایی کردن خرید</a></td></tr>')

                    $("#mobileDiscountReportage").html('تخفیف ' + data.DiscountAmount);
                    $("#mobileTotalReportage").html('ق.پرداختی ' + data.Total + ' تومان');
                }
            }

        },
        error: function () {
            console.log(result);
            alert('errot');
        }

    });



}

function loginUser() {
    //DisappearButton();
    var cellNumber = $('#cellnumber').val();
    var pass = $('#password').val();


    var basketType = getUrlVars()["basketType"];

    if (pass !== "" && cellNumber !== "") {
        $.ajax(
            {
                url: "/Account/LoginFromBasket",
                data: {
                    cellNumber: cellNumber,
                    password: pass
                },
                type: "GET"
            }).done(function (result) {
                if (result === "true") {
                    window.location = '/basket?basketType=' + basketType;
                }
                else if (result === "invalid") {
                    $('#error-box').css('display', 'block');
                    $('#error-box').html('نام کاربری یا کلمه عبور صحیح نمی باشد');
                    AppearButton();

                } else {
                    $('#error-box').css('display', 'block');
                    $('#error-box').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                    AppearButton();
                }
            });
    }
    else {
        $('#error-box').css('display', 'block');
        $('#error-box').html('شماره موبایل و کلمه عبور را وارد نمایید.');
        //AppearButton();

    }

}

function finalizeOrder() {
    //DisappearButton();
    //var billingCellnumber = $('#billing_cellnumber').val();
    //var billingFullname = $('#billing_fullname').val();
    //var billingEmail = $('#billing_email').val();

    var basketType = getUrlVars()["basketType"];

    //if (billingFullname !== "" && billingCellnumber !== "") {
    $.ajax(
        {
            url: "/Basket/Finalize",
            data: {
                basketType: basketType
            },
            type: "GET"
        }).done(function (result) {
            if (result !== "false") {
                window.location = result;
            }
            else {
                $('#error-box-final').css('display', 'block');
                $('#error-box-final').html('خطایی رخ داده است. لطفا مجددا تلاش کنید');
                AppearButton();

            }
        });
    //}
    //else {
    //    $('#error-box').css('display', 'block');
    //    $('#error-box').html('تمامی فیلدهای ستاره دار باید تکمیل شود');
    //    //AppearButton();

    //}

}

function deleteCookie(name) {
    //document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = name + '=; Max-Age=-99999999;';  
}

  

function search() {
    var searchTerm = $('#TextSearch').val();

    //window.location = "/product?searchterm=" + searchTerm;

    //window.location.replace("/product?searchterm=" + searchTerm);
    //return false;


    window.onload = function () {
        document.getElementById("search-form").onsubmit = function () {
            window.location.replace("/product?searchterm=" + searchTerm);
            return false;
        }
    }
}

function postNewsletter() {
    var newsletter = $('#txtNewsletter').val();
    if (newsletter === '') {
        $('#danger-alert-nl').css('display', 'block');
        $('#success-alert-nl').css('display', 'none');
        $('#danger-alert-nl').html('ایمیل خود را وارد نمایید');
    } else {
        $.ajax(
            {
                url: "/Newsletters/EmailPost",
                data: { email: newsletter },
                type: "Post"
            }).done(function (result) {
                if (result === "true") {
                    $('#danger-alert-nl').css('display', 'none');
                    $('#success-alert-nl').css('display', 'block');
                } else if (result == "invalidemail") {
                    $('#danger-alert-nl').css('display', 'block');
                    $('#success-alert-nl').css('display', 'none');
                    $('#danger-alert-nl').html('ایمیل وارد شده صحیح نمی باشد');
                } else {
                    $('#danger-alert-nl').css('display', 'block');
                    $('#success-alert-nl').css('display', 'none');
                    $('#danger-alert-nl').html('خطایی رخ داده است. لطفا دقایقی دیگر مجدادا تلاش کنید');
                }
            });
    }
}

function removeFromBasket(id,productType) {
    $.ajax(
        {
            url: "/basket/RemoveFromBasket",
            data: { code: id, productType: productType },
            type: "Post"
        }).done(function (result) {
            if (result === "true") {
                location.reload();

            } else {
                alert('خطایی رخ داده است. لطفا دقایقی دیگر مجدادا تلاش کنید');
            }
        });
}