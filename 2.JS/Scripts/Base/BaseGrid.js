class BaseGrid {
    constructor(gridId, formId, warningFormId) {
        let me = this;

        me.grid = $(gridId);
        me.form = $(formId);
        me.warn = $(warningFormId);

        //Lấy dữ liệu từ server
        me.getDataServer();

        //Khởi tạo các sự kiện
        me.initEvent();

        //di chuyển form
        me.draggableForm();
    }

    /**
     * Hàm khởi tạo các sự kiện
     * Ngọc 23/07/2021
     */
    initEvent() {
        let me = this;

        //Khởi tạo sự kiện cho nút thêm mới
        me.add();

        //Khởi tạo sự kiện dblclick để sửa
        me.edit();

        //Khởi tạo sự kiện click chuội trái rồi sang chuột phải thì xóa
        me.delete();

        //Khởi tạo sự kiện refresh trang
        me.refresh();

        me.close();
    }

    /**
     * Hàm lấy tất cả dữ liệu từ Server
     * Ngọc 22/07/2021
     */
    getDataServer() {
        let me = this,
            url = me.grid.attr("Url"),
            urlFull = `http://cukcuk.manhnv.net/${url}`;

        $.ajax({
            url: urlFull,
            method: "GET",
            success: function(response) {
                me.loadData(response);
                updateEntityNumber(response.length);
            },
            error: function(error) {
                console.log(error);
            },
        });
    }

    /**
     * Hàm dùng để render dữ liệu vào bảng
     * Ngọc 22/7/2021
     */
    loadData(data) {
        let me = this,
            table = $("<table></table>"),
            thead = me.renderHeader(),
            tbody = me.renderBody(data);

        table.append(thead);
        table.append(tbody);

        me.grid.find("table").remove();
        me.grid.append(table);
    }

    /**
     * Hàm dùng để render thead
     * Ngọc 22/07/2021
     */
    renderHeader() {
        let me = this,
            thead = $("<thead></thead>"),
            tr = $("<tr></tr>");

        me.grid.find(".column").each(function() {
            let text = $(this).text(),
                th = $("<th></th>");

            th.text(text);
            tr.append(th);
        });

        thead.append(tr);

        return thead;
    }

    /**
     * Hàm dùng để render tbody
     * Ngọc 22/07/2021
     */
    renderBody(data) {
        let me = this,
            tbody = $("<tbody></tbody>"),
            itemId = me.grid.attr("ItemId");

        if (data && data.length > 0) {
            data.forEach(function(item) {
                let tr = $("<tr></tr>");

                tr.attr("itemId", `${item[itemId]}`);

                me.grid.find(".column").each(function() {
                    let td = $("<td></td>"),
                        fieldName = $(this).attr("FieldName"),
                        dataType = $(this).attr("DataType"),
                        data = item[fieldName],
                        className = me.getClassName(dataType),
                        value = me.getValue(data, dataType, $(this));

                    td.text(value);
                    td.addClass(className);
                    tr.append(td);
                });

                tbody.append(tr);
            });
        }
        return tbody;
    }

    /**
     * Hàm lấy class format cho từng kiểu dữ liệu
     * Ngọc 22-07-2021
     * @param {Hàm} dataType
     */
    getClassName(dataType) {
        let me = this,
            className = "";

        switch (dataType) {
            case "Number":
                className = "ta-r";
                break;
            case "Date":
                className = "ta-center";
                break;
            default:
                className = "ta-l";
                break;
        }

        return className;
    }

    /**
     * Hàm format từng giá trị của dữ liệu thô theo convention
     * Ngọc 23/07/2021
     */
    getValue(data, dataType, column) {
        let me = this;

        switch (dataType) {
            case "Number":
                data = CommonFn.formatMoney(data);
                break;
            case "Date":
                data = CommonFn.formatDate(data);
                break;
            case "Enum":
                let enumName = column.attr("EnumName");
                data = CommonFn.getValueEnum(data, enumName);
                break;
        }

        return data;
    }

    /**
     * Hàm thêm mới dữ liệu
     * Ngọc 23/07/2021
     */
    add() {
        let me = this;

        $(".content-header #btnAdd").click(function() {
            //$(".p-absolute").show();
            me.form.show();
            $(".wrapper").addClass("fade");

            let toolBar = $(this).attr("Toolbar");

            //Hàm reset các trường
            me.resetPopup(toolBar);

            me.SaveData(0);
        });
    }

    /**
     * Hàm tự focus vào ô mã rồi tạo 1 mã mới
     * Ngọc 23/07/2021
     * (focus chưa hoạt động)
     */
    resetPopup(toolBar) {
        let me = this,
            item = me.form.attr("Item"),
            url = me.grid.attr("Url");
        // formMode = me.form.attr("FormMode");

        $.ajax({
                url: `http://cukcuk.manhnv.net/${url}/${toolBar}`,
                method: "GET",
            })
            .done((res) => {
                me.form.find(`input[FieldName = ${item}]`).val(res);
                me.form.find(`input[FieldName = ${item}]`).focus();
            })
            .fail((err) => {
                $(".toast.message-yellow").slideDown();
                $(".toast.message-yellow").find(".toast-text").text("Chưa tạo được mã nhân viên mới!")
                console.log(err);
            });

        me.form.find("[FieldName]").each(function() {
            let cell = $(this),
                dataType = cell.attr("DataType"),
                fieldName = cell.attr("FieldName");

            if (fieldName == item) {
                return;
            } else if (dataType == "Enum") {
                cell.attr("Value", "-1");
                cell.text("Chưa chọn");
            } else {
                cell.val("");
            }
        });
    }

    /**
     * Lấy dữ liệu từng ô trong form dựa vào DataType
     * Ngọc 23/07/2021
     */
    getValueForm(cell, dataType) {
        let me = this,
            value = "";

        switch (dataType) {
            case "Date":
                value = new Date(cell.val());
                break;
            case "Number":
                value = CommonFn.formatNumber(cell.val());
                break;
            case "Enum":
                value = cell.attr("Value");
                break;
            default:
                value = cell.val();
                break;
        }
        return value;
    }

    /**
     * Hàm sửa dữ liệu
     * Ngọc 23/07/2021
     */
    edit() {
        let me = this,
            url = me.grid.attr("Url");

        me.grid.on("dblclick", "table tbody tr", function() {
            let itemId = $(this).attr("itemId");
            me.form.show();
            $(".wrapper").addClass("fade");
            $.ajax({
                    url: `http://cukcuk.manhnv.net/${url}/${itemId}`,
                    method: "GET",
                })
                .done((res) => {
                    try {
                        me.form.find("[FieldName]").each(function() {
                            let cell = $(this),
                                fieldName = cell.attr("FieldName"),
                                dataType = cell.attr("DataType"),
                                value = res[fieldName];

                            me.setValueForm(value, dataType, cell);
                        });
                    } catch (err) {
                        console.log(err);
                    }
                })
                .fail(function(err) {
                    console.log(err);
                });

            me.SaveData(1, itemId);
        });
    }

    /**
     * Hàm xử lí sự kiện bấm nút Lưu
     * Ngọc 23/07/2021
     */
    SaveData(formMode, itemId) {
        let me = this,
            url = me.grid.attr("Url");

        me.validateEmptyField();
        $("#btnSave").click(function() {

            let isValid = me.validateForm(formMode, itemId);
            if (isValid) {
                let entity = {},
                    urlFull = "",
                    method = "",
                    message = "",
                    success = true;

                me.form.find("[FieldName]").each(function() {
                    let cell = $(this),
                        dataType = cell.attr("DataType"),
                        fieldName = cell.attr("FieldName"),
                        value = me.getValueForm(cell, dataType);

                    entity[fieldName] = value;
                });

                if (formMode == 0) {
                    urlFull = `http://cukcuk.manhnv.net/${url}`;
                    method = "POST";
                    message = "Thêm mới thành công";
                } else {
                    urlFull = `http://cukcuk.manhnv.net/${url}/${itemId}`;
                    method = "PUT";
                    message = "Sửa thành công";
                }
                $.ajax({
                    url: urlFull,
                    method: method,
                    data: JSON.stringify(entity),
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                }).done((res) => {
                    $(".toast.message-green").slideDown("slow");
                    $(".toast.message-green").find(".toast-text").text(message);

                }).fail(function(res) {
                    success = false;
                    console.log(res);
                    if (res.responseJSON.devMsg.includes("Duplicate")) {
                        alert("Mã nhân viên bị trùng")
                    }
                });
                //location.reload();

            }

        });
    }

    /**
     * Hàm kiển tra dữ liệu
     * Ngọc 2-6-2021
     */
    validateForm(formMode, itemId) {
        let me = this,
            isValid = me.validateRequire();

        if (isValid) {
            isValid = me.validateCode(formMode, itemId);
        }

        if (isValid) {
            isValid = me.validateDropdown();
        }

        if (isValid) {
            isValid = me.validateFieldNumber();
        }

        if (isValid) {
            isValid = me.validateFieldDate();
        }

        if (isValid) {
            isValid = me.validateEmail();
        }

        if (isValid) {
            isValid = me.validatePhoneNumber();
        }

        if (isValid) {
            isValid = me.validateOnlyNumber();
        }

        if (isValid) {
            isValid = me.validateCustom();
        }

        return isValid;
    }

    /**
     * 
     * @returns 
     */
    validateCustom() {
        return true;
    }

    /**
     * hàm validate dữ liệu (nhập các trường bắt buộc)
     * Ngọc 20/7/2021
     */
    validateRequire() {

        let me = this,
            isValid = true,
            str = "";

        // Duyệt hết các trường require xem có trường nào bắt buộc mà ko có value ko
        me.form.find("input[required]").each(function() {
            let inp = $(this),
                value = inp.val();

            if (!value) {
                isValid = false;
                //inp.parent().find("label span").remove();
                // let text = inp.parent().find("label").text();
                // str += `${text} \n`;
                // inp.parent().find("label").html(`<label>${text}<span class='cl-red'>(*)</span></label>`);
                inp.addClass("notValidControl");
                inp.attr("title", `Vui lòng không được để trống!`);
            } else {
                // if (!inp.parent().find("label span")) {
                //     inp.parent().find("label").append(span);
                // }
                inp.removeClass("notValidControl");
            }
        });
        if (isValid == false) {
            $(".toast.message-red").slideDown();
            $(".toast.message-red").find(".toast-text").text("Vui lòng không để trống các trường bắt buộc!")
                // alert(`${str}Không được bỏ trống`)
        }
        return isValid;
    }

    /**
     * Hàm Validate các dropdown
     * Ngọc 24/07/2021 
     */
    validateDropdown() {
        let me = this,
            isValid = true,
            str = "";

        // Duyệt hết các trường require xem có trường nào bắt buộc mà ko có value ko
        me.form.find("[DataType='Enum']").each(function() {
            let value = $(this).attr("Value");
            //let text = $(this).parent().parent().find("label").text();

            // is not a number
            if (value == -1) {
                isValid = false;
                // str += `${text}\n`;
                $(this).parent().addClass("notValidControl");
                $(this).attr("title", "Vui lòng chọn các lựa chọn!");
            } else {
                $(this).parent().removeClass("notValidControl");
            }
        });
        if (isValid == false) {
            // $(".toast.message-red").removeClass("d-none");
            $(".toast.message-red").slideDown("slow");
            $(".toast.message-red").find(".toast-text").text("Vui lòng chọn giá trị cho các ô chưa chọn!")
                //alert(`Vui lòng chọn giá trị cho ${str}`);
        }


        return isValid;
    }

    /**
     * Hàm Validate các trường Number
     * Ngọc 24/07/2021
     * @returns true/false
     */
    validateFieldNumber() {
        let me = this,
            isValid = true;

        me.form.find("[DataType='Number']").each(function() {
            let value = $(this).val();

            // is not a number
            if (isNaN(CommonFn.formatNumber(value))) {
                isValid = false;

                $(this).addClass("notValidControl");
                $(this).attr("title", "Vui lòng nhập đúng định dạng!");
            } else {
                $(this).removeClass("notValidControl");
            }
        });

        return isValid;
    }

    /**
     * Hàm Validate các trường ngày tháng
     * Ngọc 24/07/2021 
     */
    validateFieldDate() {
        let me = this,
            isValid = true;

        // Duyệt hết các trường Date 
        me.form.find("[DataType='Date']").each(function() {
            let inpDate = $(this),
                value = inpDate.val();

            if (!CommonFn.isDateFormat(value)) {
                isValid = false;

                inpDate.addClass("notValidControl");
                inpDate.attr("title", "Vui lòng nhập đúng định dạng!");
            } else {
                inpDate.removeClass("notValidControl");
            }
        });
        if (isValid == false) {
            $(".toast.message-red").slideDown();
            $(".toast.message-red").find(".toast-text").text("Vui lòng nhập đúng định dạng ngày tháng!")
                // alert(`${str}Không được bỏ trống`)
        }
        return isValid;
    }


    /**
     * hàm kiểm tra dữ liệu sau khi nhập (nhập các trường bắt buộc)
     * Ngọc 20/7/2021
     */
    validateEmptyField() {
        $('input[required]').blur(function() {
            let me = $(this)
            let value = me.val();
            if (value == '') {
                me.css('border', '1px solid red');
                me.attr('title', 'Thông tin này bắt buộc nhập');
            } else {
                me.css('border', '1px solid #bbbbbb');
                me.removeAttr('tittle')
            }
            me.focus(function() {
                me.css("border", "1px solid #01B075");
            })
        })
    }

    /**
     * Hàm lấy dữ liệu thô để gán vào form
     * Ngọc 23/07/2021
     */
    setValueForm(value, dataType, cell) {
        let me = this;

        switch (dataType) {
            case "Date":
                cell.val(CommonFn.convertDate(value));
                break;
            case "Number":
                cell.val(CommonFn.formatMoney(value));
                break;
            case "Enum":
                cell.attr("Value", `${value}`);
                cell.text(
                    cell
                    .parent()
                    .parent()
                    .find(`.dropdown-item[value='${value}'] .dropdown-text`)
                    .text()
                );
                //Các dropdown được tích theo tên tương ứng
                me.dropdownSelected(cell, value);
                break;
            default:
                cell.val(value);
                break;
        }
    }

    /**
     * Hàm check dropdown-item nào có value giống value của div.inp
     * Ngọc 23/07/2021
     */
    dropdownSelected(cell, value) {
        let me = this;

        cell
            .parent()
            .parent()
            .find(`.dropdown-item[value='${value}']`)
            .addClass("bg-select");
    }

    /**
     * Hàm xóa dữ liệu
     * Ngọc 23/07/2021
     */
    delete() {
        let me = this,
            url = me.grid.attr("Url");

        me.trToggleSelected();

        me.grid.on("mousedown", "table tbody tr.tr-select", function(e) {
            let allTrSelect = $("table tbody tr.tr-select");

            console.log(allTrSelect.length);
            if (e.which == 3) {
                alert(`Mở form Xóa ${me.entityName}`);
                $(".p-absolute").show();
                me.warn.show();
                $(".wrapper").addClass("fade");
                let itemId = "";
                if (allTrSelect.length == 1) {
                    allTrSelect.each(function() {
                        itemId = $(this).attr("itemId");
                    });
                    $.ajax({
                            url: `http://cukcuk.manhnv.net/${url}/${itemId}`,
                            method: "GET",
                        })
                        .done((res) => {
                            me.warn
                                .find(".head .head-text")
                                .text(`Xóa thông tin ${me.entityName} ${res.FullName}`);
                            me.warn
                                .find(".main .text")
                                .html(
                                    `Bạn có chắc muốn xóa thông tin của ${me.entityName} <b>${res.FullName}</b> này không`
                                );
                        })
                        .fail(function(err) {
                            console.log(err);
                        });

                    $("#btnConfirm").click(function() {
                        $.ajax({
                                url: `http://cukcuk.manhnv.net/${url}/${itemId}`,
                                method: "DELETE",
                            })
                            .done((res) => {
                                $(".toast.message-green").slideDown("slow");
                                $(".toast.message-green").find(".toast-text").text("Xóa dữ liệu thành công");
                                setTimeOut(me.reload, 1000);
                            })
                            .fail(function(res) {
                                console.log(res);
                            });

                    });
                }

                if (allTrSelect.length > 1) {
                    me.warn
                        .find(".head .head-text")
                        .text(`Xóa thông tin của ${allTrSelect.length} ${me.entityName}`);
                    me.warn
                        .find(".main .text")
                        .html(
                            `Bạn có chắc muốn xóa thông tin của  <b>${allTrSelect.length}</b> ${me.entityName} này không`
                        );

                    $("#btnConfirm").click(function() {
                        let completed = true;

                        allTrSelect.each(function() {
                            itemId = $(this).attr("itemId");
                            $.ajax({
                                    url: `http://cukcuk.manhnv.net/${url}/${itemId}`,
                                    method: "DELETE",
                                })
                                .done((res) => {

                                })
                                .fail(function(res) {
                                    completed = false;
                                    console.log("Xóa thất bại");
                                    console.log(res);
                                });
                        });

                        if (completed) {
                            $(".toast.message-green").slideDown("slow");
                            $(".toast.message-green").find(".toast-text").text("Xóa dữ liệu thành công");
                            me.reload();
                        }
                    });
                }
            }
        });
    }

    /**
     * Hàm click chọn tr
     * Ngọc 21/07/2021
     */
    trToggleSelected() {
        let me = this;
        me.grid.on("click", "table tbody tr", function() {
            $(this).toggleClass("tr-select");
        });
    }

    /**
     * Hàm bấm refresh trang
     * Ngọc 23/07/2021
     */
    refresh() {
        let me = this;
        $(".refresh").click(function() {
            location.reload();
        });
    }

    /**
     * Hàm reload trang
     * Ngọc 25/07/2021
     */
    reload() {
        let me = this;

        location.reload();
    }

    /**
     * Hàm đóng popup,warning-popup
     * Ngọc 23/07/2021
     */
    close() {
        let me = this;

        $(".head-close, .button.cancel").click(function() {
            //reset border các ô input
            me.form.find(".textbox-default").css("border", "1px solid #bbbbbb");
            //reset bỏ chọn các dropdown-item
            me.form.find(".dropdown-item").removeClass("bg-select");
            $(".p-absolute").hide();
            me.warn.hide();
            me.form.hide();
            $(".wrapper").removeClass("fade");
            $(".X").attr("style", "visibility: hidden;");
        });
    }


    /**
     * Hàm cho phép kép thả form
     * Ngọc 24/07/2023
     */
    draggableForm() {
        let me = this;
        me.form.draggable({ handle: ".head" });
        me.warn.draggable({ handle: ".head" });
    }
}