$(".toast-close").click(function() {
    $(this).parent().parent().slideUp();
})

$(".message-green .toast-close").click(function() {
    location.reload();
})