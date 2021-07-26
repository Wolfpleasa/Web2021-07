/**
 * Nếu các ô required đã có giá trị thì bỏ viền đỏ
 * Ngọc 25/07/2021
 */
$('input[required]').on("input", function() {
    let value = $(this).val();
    if (value) {
        $(this).removeClass("notValidControl");
        $(this).removeAttr("title");
    }
})

/**
 * Nếu các ô Date đã đúng định dạng bỏ viền đỏ
 * Ngọc 25/07/2021
 */
$("[DataType='Date']").on("input", function() {
    let value = $(this).val();

    if (CommonFn.isDateFormat(value)) {
        $(this).removeClass("notValidControl");
        $(this).removeAttr("title");
    }
})

/**
 * Nếu ô lương đã đúng định dạng bỏ viền đỏ
 * Ngọc 25/07/2021
 */
$("[DataType='Number']").on("input", function() {
    let value = $(this).val();

    if (!isNaN(CommonFn.formatNumber(value))) {
        $(this).removeClass("notValidControl");
        $(this).removeAttr("title");
    }
})

/**
 * Nếu các ô chỉ chứa chữ số đã đúng định dạng bỏ viền đỏ
 * Ngọc 25/07/2021
 */
$('[DataType = "OnlyNumber"]').on("input", function() {
    let value = $(this).val();

    if (CommonFn.onlyNumber(value)) {
        $(this).removeClass("notValidControl");
        $(this).removeAttr("title");
    }
})

/**
 * Nếu ô SĐT đã đúng định dạng bỏ viền đỏ
 * Ngọc 25/07/2021
 */
$('[FieldName = "Email"]').on("input", function() {
    let value = $(this).val();
    var at = value.indexOf("@");
    var dot = value.lastIndexOf(".");
    var space = value.indexOf(" ");
    if ((at != -1) && //có ký tự @
        (at != 0) && //ký tự @ không nằm ở vị trí đầu
        (dot != -1) && //có ký tự .
        (dot > at + 1) && (dot < value.length - 1) //phải có ký tự nằm giữa @ và . cuối cùng
        &&
        (space == -1)) //không có khoẳng trắng 
    {
        $(this).removeClass("notValidControl");
        $(this).removeAttr("title");
    }
})

/**
 * Nếu ô Email đã đúng định dạng bỏ viền đỏ
 * Ngọc 25/07/2021
 */
$('[FieldName = "PhoneNumber"]').on("input", function() {
    let value = $(this).val();

    if (value.length == 10) {
        $(this).removeClass("notValidControl");
        $(this).removeAttr("title");
    }
})