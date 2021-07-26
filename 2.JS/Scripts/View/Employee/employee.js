// Trang nhân viên
class EmployeePage extends BaseGrid {
    constructor(gridId, formId, warningFormId) {
        super(gridId, formId, warningFormId);

        // this.getPosition();
        // this.getDepartment();
        // this.resetPopup();
        this.config();
    }

    config() {
        let me = this,
            param = {
                entityName: "nhân viên",
            }
        Object.assign(me, param);
    }

    /**
     * Hàm lấy dữ liệu phòng ban cho dropdown
     * Ngọc 20/7/2021
     */
    getPosition() {
        $.ajax({
            url: "http://cukcuk.manhnv.net/v1/Positions",
            method: "GET",
        }).done((res) => {
            res.forEach((position) => {
                const positionName = position["PositionName"];
                const positionId = position["PositionId"];
                let dropdown = $(".dropdown.dd-Position");
                let item = `<div class="dropdown-item" Value = "${positionId}">
                <div class="dropdown-icon"></div>
                <div class="dropdown-text" >${positionName}</div>
                </div>`;
                dropdown.append(item);
            });
        });
    }

    /**
     * Hàm lấy dữ liệu phòng ban cho dropdown
     * Ngọc 20/7/2021
     */
    getDepartment() {
        $.ajax({
            url: "http://cukcuk.manhnv.net/api/Department",
            method: "GET",
        }).done((res) => {
            res.forEach((department) => {
                const departmentName = department["DepartmentName"];
                const departmentId = department["DepartmentId"];
                let dropdown = $(".dropdown.dd-Department");
                let item = `<div class="dropdown-item" Value = "${departmentId}">
                <div class="dropdown-icon"></div>
                <div class="dropdown-text" >${departmentName}</div>
                </div>`;
                dropdown.append(item);
            });
        });
    }

    /**
     * Hàm xử lí email 
     * Ngọc 24/07/2021
     */
    validateEmail() {
        let me = this,
            isValid = true;

        me.form.find('[FieldName = "Email"]').each(function() {
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
            } else {
                alert("Email không đúng định dạng");
                $(this).addClass("notValidControl");
                $(this).attr("title", "Vui lòng nhập đúng định dạng Email!");
                isValid = false;
            }
        });

        return isValid
    }

    /**
     * Hàm kiểm tra số điện thoại
     * Ngọc 24/07/2021
     */
    validatePhoneNumber() {
        let me = this,
            isValid = true;

        me.form.find('[FieldName = "PhoneNumber"]').each(function() {
            let value = $(this).val();
            if (value.length != 10) {
                isValid = false;
                alert("SĐT không đúng định dạng");
                $(this).addClass("notValidControl");
                $(this).attr("title", "Vui lòng nhập đúng định dạng SĐT!");
            } else {
                $(this).removeClass("notValidControl");
            }
        })

        return isValid;
    }

    /**
     * Hàm kiểm tra các trường chỉ chứa chữ số
     * Ngọc 24/07/2021
     */
    validateOnlyNumber() {
        let me = this,
            isValid = true,
            str = "";

        me.form.find('[DataType = "OnlyNumber"]').each(function() {
            let value = $(this).val();
            let label = $(this).parent().find("label").text();

            if (!CommonFn.onlyNumber(value)) {
                isValid = false;
                str += `${label} `;
                $(this).addClass("notValidControl");
                $(this).attr("title", "Ô này chỉ chứa chữ số");

            } else {
                $(this).removeClass("notValidControl");
            }
        })

        if (isValid == false) {
            $(".toast.message-red").slideDown();
            $(".toast.message-red").find(".toast-text").text(`${str}chỉ chứ chữ số`);
            //alert(`${str}chỉ chứ chữ số`);
        }
        return isValid;
    }

    /**
     * Hàm kiểm tra mã
     * Ngọc 24/07/2021
     * @param {*} formMode: xác định là hàm thêm hay sửa đang gọi 
     * @param {*} itemId: employeeId của nhân viên nếu hàm sửa gọi 
     * @returns true/false
     */
    validateCode(formMode, itemId) {

        let me = this,
            url = me.grid.attr("Url"),
            isValid = true,
            pageNumber = 1,
            pageSize = 100;

        me.form.find('[FieldName = "EmployeeCode"]').each(function() {
            let employeeCode = $(this).val(),
                label = $(this).parent().find("label").text();
            $.ajax({
                type: "GET",
                url: `http://cukcuk.manhnv.net/${url}/filter?pageNumber=${pageNumber}&pageSize=${pageSize}&employeeCode=${employeeCode}`,
                success: function(response) {
                    let result = response;
                    if (response) {
                        if (formMode == 0) {
                            isValid = false;
                            $(".toast.message-red").slideDown();
                            $(".toast.message-red").find(".toast-text").text(`${label} bị nhập trước rồi :))`);
                            $(this).addClass("notValidControl");
                            $(this).attr("title", "Ô này chỉ chứa chữ số");
                        } else {
                            if (response['EmployeeId'] !== itemId) {
                                isValid = false;
                                $(".toast.message-red").slideDown();
                                $(".toast.message-red").find(".toast-text").text(`${label} bị nhập trước rồi :))`);
                            }
                        }
                    } else {
                        isValid = false;
                    }
                },
            }).fail(err => {
                console.log(err);
            })

            $(this).removeClass("notValidControl");
        })

        return isValid;
    }

}

let employeePage = new EmployeePage("#gridEntity", "#popup", "#warning-popup");
employeePage.getDepartment();
employeePage.getPosition();

//employeePage.resetPopup();