$(document).ready(function () {
  /* toggle header-menu  */
  $("#toggle-menu-button").click(function () {
    $(".header-section .bottom-header-box .menu-box").toggleClass("active");
  });

  /* toggle sub-menu-item of header-menu  */
  if (window.matchMedia("(min-width: 991px)").matches) {
    $(".header-section .menu-item.drop-item .menu-link").mouseover(function () {
      $(this).parent().find(".sub-menu").addClass("active");
    });
    $(".header-section .menu-item.drop-item .menu-link").mouseleave(
      function () {
        $(this).parent().find(".sub-menu").removeClass("active");
      }
    );
    $(
      ".header-section .menu-item.drop-item .sub-menu .sub-menu-link"
    ).mouseover(function () {
      $(this)
        .parent()
        .parent(".header-section .menu-item.drop-item .sub-menu")
        .addClass("active");
    });
    $(
      ".header-section .menu-item.drop-item .sub-menu .sub-menu-link"
    ).mouseleave(function () {
      $(this)
        .parent()
        .parent(".header-section .menu-item.drop-item .sub-menu")
        .removeClass("active");
    });
  } else if (window.matchMedia("(max-width: 990.999px)").matches) {
  /* toggle sub-menu-item of header-menu  in responsive */
    $(".header-section .menu-item.drop-item .menu-link").click(function () {
      $(this).parent().find(".sub-menu").toggleClass("activeCollapse");
    });
  }

  /* scroll to top  */
  $("#scroll-top-bottun").click(function () {
    window.scrollTo({
      top: 10 ,
      left: 10,
      behavior: "smooth",
    });
  });
});
