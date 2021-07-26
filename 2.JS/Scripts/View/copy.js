/**
 * Hàm mở ảnh trong máy 
 * Ngọc 16/7/2021
 */
$('.image').click(function() {
    $('#myFile').trigger('click');
})


/**
 * Hàm load ảnh trong máy lên form 
 * Ngọc 16/7/2021
 */
$('#myFile').click(function(e) {
    $('#myFile').change(function(e) {
        var img = URL.createObjectURL(e.target.files[0]);
        $('.image').css("background-image", `url(${img})`);
    })
})