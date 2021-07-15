$(".content-header .button").click(function() {
    $(".popup").css('visibility', 'visible');
    $("#focus").focus();
    $(this).attr("disable", 'true');
});

$(".head-close").click(function() {
    $(".popup").css('visibility', 'hidden');
})

$(".button.cancel").click(function() {
    $(".popup").css('visibility', 'hidden');
})

$(".refresh").click(function() {
    location.reload();
})

$('.image').click(function() {
    $('#myFile').trigger('click');
})

$('#myFile').click(function(e) {
    $('#myFile').change(function(e) {
        var img = URL.createObjectURL(e.target.files[0]);
        $('.image').css("background-image", `url(${img})`);
    })
})

//dropdown
$(".select-arrow").click(function() {
    let $item = $(this)

    if ($item.hasClass("down")) {
        $item.removeClass("down");
        $(this).parent().parent().find(".dropdown").attr("style", "display: block;");
    } else {
        $item.addClass("down");
        $(this).parent().parent().find(".dropdown").attr("style", "display: none;");
    }
})
$(".dropdown-item").click(function() {
    console.log("item");
    let val = $(this).find(".dropdown-text").text();
    console.log(val);
    $(this).parents('.dropdown').find('.dropdown-item').removeClass("bg-select");
    $(this).addClass("bg-select");
    $(this).parent().parent().find(".inp").text(val);
    $(this).parent().parent().find(".X").attr("style", "visibility: visible;");
})

$(document).on("click", function(e) {
    let $el = $(e.target),
        isArrow = $el.data("select-arrow");

    if (!isArrow) {
        $(".dropdown").hide();
        // $(".content-control .arrow").removeClass("down");
    }
})