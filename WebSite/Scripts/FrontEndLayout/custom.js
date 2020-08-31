//$(document).ready(function () {

//    alert("Your screen resolution is: " + screen.width + "x" + screen.height);

//});
function SendMessage() {
    var nameVal = $("#txtName").val();
    var cellNumVal = $("#txtCellNum").val();
    var msgVal = $("#txtBody").val();

    if (nameVal !== "", cellNumVal !== "", msgVal !== "") {
        $.ajax(
            {
                url: "/home/SendMessage",
                data: { name: nameVal, cellNum: cellNumVal, message: msgVal },
                type: "GET"
            }).done(function (result) {
                if (result == "true") {
                    document.getElementById('txtName').value = "";
                    document.getElementById('txtCellNum').value = "";
                    document.getElementById('txtBody').value = "";
                    alert('پیغام شما با موفقیت ثبت گردید');
                }
                else if (result == "false") {
                    alert('خطایی رخ داد! مجددا تلاش نمایید');
                }
            });
    }
    else {
        alert('تمامی موارد درخواستی را تکمیل نمایید');
    }
}

function JoinNewsLetter() {
    var emailVal = $("#txtEmail").val();
    if (emailVal != "") {
        $.ajax(
            {
                url: "/home/JoinNewsLetter",
                data: { email: emailVal },
                type: "GET"
            }).done(function (result) {
                if (result == "true") {
                    document.getElementById('txtEmail').value = "";
                    alert('عضویت شما در خبرنامه با موفقیت صورت پذیرفت');
                }
                else if (result == "false") {
                    alert('خطایی رخ داد! مجددا تلاش نمایید');
                }
                else if (result == "InvalidEmail") {
                    alert('ایمیل وارد شده صحیح نمی باشد! مجددا تلاش نمایید');
                }
            });
    }
    else {
        alert('جهت عضویت در خبرنامه ایمیل خود را وارد نمایید');
    }
}

function SubmitComment(id) {

    var emailVal = $("#txtEmail").val();
    var fullnameVal = $("#txtFullname").val();
    var messageVal = $("#txtMessage").val();

    if (emailVal != "", fullnameVal != "", messageVal != "") {
        $.ajax(
            {
                url: "/Blogs/SubmitComment",
                data: { id: id, email: emailVal, name: fullnameVal, message: messageVal },
                type: "GET"
            }).done(function (result) {
                if (result == "true") {
                    document.getElementById('txtEmail').value = "";
                    $("#SuccessDivC").css('display', 'block');
                    $("#errorDivC").css('display', 'none');
                }
                else if (result == "false") {
                    $("#SuccessDivC").css('display', 'none');
                    $("#errorDivC").css('display', 'block');
                    document.getElementById("errorDivC").innerHTML = "خطایی رخ داد! مجددا تلاش نمایید";
                }
                else if (result == "InvalidEmail") {
                    $("#SuccessDivC").css('display', 'none');
                    $("#errorDivC").css('display', 'block');
                    document.getElementById("errorDivC").innerHTML = "ایمیل وارد شده صحیح نمی باشد! مجددا تلاش نمایید";
                }

            });
    }
    else {
        $("#SuccessDivC").css('display', 'none');
        $("#errorDivC").css('display', 'block');
        document.getElementById("errorDivC").innerHTML = "جهت ثبت یادداشت تمامی موارد درخواستی را تکمیل نمایید";
    }
}

function SubmitRequest() {
    var siteVal = $("#txtSite").val();
    var emailVal = $("#txtEmail").val();
    var siteTypeVal = $('select#SiteType option:selected').val();
    var serviceTypeVal = $('select#ServiceType option:selected').val();
    var serviceTypeVal = $('select#ServiceType option:selected').val();
}

function SendRequest(id) {
    var siteVal = $("#txtSite").val();
    var emailVal = $("#txtEmail").val();
    var phoneVal = $("#txtPhone").val();
    var mainWordVal = $("#txtMainWord").val();
    var descVal = $("#txtDesc").val();

    var siteTypeVal = $('select#SiteTypeId option:selected').val();
    var serviceTypeVal = $('select#ServiceTypeId option:selected').val();

    if (siteVal != "", emailVal != "", mainWordVal != "", descVal != "", siteTypeVal != "", serviceTypeVal != "") {
        $.ajax(
            {
                url: "/ServiceGroups/InsertServiceForm",
                data: { site: siteVal, email: emailVal, mainWord: mainWordVal, desc: descVal, siteType: siteTypeVal, serviceType: serviceTypeVal, id: id, phone: phoneVal },
                type: "POST"
            }).done(function (result) {
                if (result == "true") {
                    $('#errorDivC').css('display', 'none');
                    $('#SuccessDivC').css('display', 'block');
                }
                else if (result == "InvalidEmail") {
                    $('#errorDivC').css('display', 'block');
                    $('#SuccessDivC').css('display', 'none');
                    $('#errorDivC').html('ایمیل وارد شده صحیح نمی باشد');
                }
                else if (result == "login") {
                    $('#errorDivC').css('display', 'block');
                    $('#SuccessDivC').css('display', 'none');
                    $('#errorDivC').html('جهت ارسال درخواست لطفا <a href="/account/login">وارد</a> سایت شوید');
                }
                else if (result == "false") {
                    $('#errorDivC').css('display', 'block');
                    $('#SuccessDivC').css('display', 'none');
                    $('#errorDivC').html('خطایی رخ داده است! مجددا تلاش نمایید');
                }
            });

    }
    else {
        $('#SuccessDivC').css('display', 'none');
        $('#errorDivC').html('تمامی موارد درخواستی را تکمیل نمایید');
        $('#errorDivC').css('display', 'block');
    }
}

function SendServiceRequest(id) {
    var siteVal = $("#txtSite").val();
    var emailVal = $("#txtEmail").val();
    var phoneVal = $("#txtPhone").val();
    var mainWordVal = $("#txtMainWord").val();
    var descVal = $("#txtDesc").val();

    var siteTypeVal = $('select#SiteTypeId option:selected').val();
    var serviceTypeVal = $('select#ServiceTypeId option:selected').val();

    if (siteVal != "", emailVal != "", mainWordVal != "", descVal != "", siteTypeVal != "", serviceTypeVal != "") {
        $.ajax(
            {
                url: "/Services/InsertServiceForm",
                data: { site: siteVal, email: emailVal, mainWord: mainWordVal, desc: descVal, siteType: siteTypeVal, serviceType: serviceTypeVal, id: id, phone: phoneVal },
                type: "POST"
            }).done(function (result) {
                if (result == "true") {
                    $('#errorDivC').css('display', 'none');
                    $('#SuccessDivC').css('display', 'block');
                }
                else if (result == "InvalidEmail") {
                    $('#errorDivC').css('display', 'block');
                    $('#SuccessDivC').css('display', 'none');
                    $('#errorDivC').html('ایمیل وارد شده صحیح نمی باشد');
                }
                else if (result == "login") {
                    $('#errorDivC').css('display', 'block');
                    $('#SuccessDivC').css('display', 'none');
                    $('#errorDivC').html('جهت ارسال درخواست لطفا <a href="/account/login">وارد</a> سایت شوید');
                }
                else if (result == "false") {
                    $('#errorDivC').css('display', 'block');
                    $('#SuccessDivC').css('display', 'none');
                    $('#errorDivC').html('خطایی رخ داده است! مجددا تلاش نمایید');
                }
            });

    }
    else {
        $('#SuccessDivC').css('display', 'none');
        $('#errorDivC').html('تمامی موارد درخواستی را تکمیل نمایید');
        $('#errorDivC').css('display', 'block');
    }
}

function SearchBlog() {
    window.location.href = "https://www.google.com/search?source=hp&ei=O6BKXeuuE4z3kwXHi7XICg&q=site%3Awww.rushweb.ir" + " " + input.value;
}

var input = document.getElementById("txtSearch");

// Execute a function when the user releases a key on the keyboard
//input.addEventListener("keyup", function (event) {
//    // Cancel the default action, if needed
//    event.preventDefault();
//    // Number 13 is the "Enter" key on the keyboard
//    if (event.keyCode === 13) {
//        // Trigger the button element with a click
//        document.getElementById("btnSearch").click();
//        window.location.href = "https://www.google.com/search?source=hp&ei=O6BKXeuuE4z3kwXHi7XICg&q=site%3Awww.rushweb.ir" + " " + input.value;
//    }
//});


function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}