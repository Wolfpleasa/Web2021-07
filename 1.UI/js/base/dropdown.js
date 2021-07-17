//dropdown
// $(".select-arrow").click(function () {
//         $(this).removeClass("rot-0");
//         $(this).addClass("rot-180");     
//         $(".dropdown").addClass("d-none");
//         $(this).parent().parent().find(".dropdown").toggleClass("d-none"); 
// })

$(".inp, .select-arrow").click(function() {
    let me = $(this),
        arrow = me.parent().find(".select-arrow"),
        dropdown = me.parent().parent().find(".dropdown");

    arrow.removeClass("rot-0");
    arrow.addClass("rot-180");
    $(".dropdown").addClass("d-none");
    dropdown.toggleClass("d-none");
});
/**
 * Bấm chọn các nội dung khác trong dropdown
 * Ngọc 16/7/2021
 */
$(".dropdown-item").click(function() {
    let me = $(this),
        val = me.find(".dropdown-text").text(),
        text = me.parent().parent().find(".inp").text();

    me.parents('.dropdown').find('.dropdown-item').removeClass("bg-select");
    me.addClass("bg-select");
    me.parent().parent().find(".inp").text(val);
    me.parent().parent().find(".X").attr("style", "visibility: visible;");
    $(".X").click(function() {
        $(this).attr("style", "visibility: hidden;")
        $(this).parent().find(".inp").text(text);
        me.removeClass("bg-select");
    })
})


/**
 * Sự kiện bấm bất kì thứ gì ngoài cái mũi tên thì dropdown ẩn đi 
 * Ngọc 16/7/2021
 */
$(document).on("click", function(e) {
    if (!$(".select-arrow").is(e.target)) {
        $(".dropdown").addClass("d-none");
        $(".select-arrow").addClass("rot-0");
        $(".select-arrow").removeClass("rot-180");
    }
})