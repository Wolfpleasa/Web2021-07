import Enumeration from "./Enumeration";
import Resource from "./Resource";

export default class CommonFn {

    /**
     * Hàm format số tiền
     * Ngọc 18/7/2021
     */
    static formatMoney(money) {
        if (money && !isNaN(money)) {
            return money.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
        } else {
            return money;
        }
    }

    /**
     * Hàm kiểm tra chuổi chỉ chứa chữ số
     * Ngọc 24/07/2021
     */
    static isNumber(number) {
        let part = /[^0-9]/g,
            res = number.match(part);
        if (!res) {
            return true;
        }
        return false;
    }

    /**
     * Hàm format số tiền về số bth
     * Ngọc 18/7/2021
     */
    static formatNumber(money) {
        let salary = money.replaceAll(".", "");
        return salary;
    }

    /**
     * Format ngày tháng để đẩy lên table vs input
     * Ngọc 18/7/2021
     */
    static formatDate(dateSrc) {
        let date = new Date(dateSrc),
            year = date.getFullYear().toString(),
            month = (date.getMonth() + 1).toString().padStart(2, "0"),
            day = date
            .getDate()
            .toString()
            .padStart(2, "0");

        return `${day}/${month}/${year}`;
    }

    /**
     * Kiểm tra xem có phải dạng date không
     * Ngọc 18/7/2021
     */
    static isDateFormat(date) {
        let regex = new RegExp(
            "([0-9]{4}[-](0[1-9]|1[0-2])[-]([0-2]{1}[0-9]{1}|3[0-1]{1})|([0-2]{1}[0-9]{1}|3[0-1]{1})[/](0[1-9]|1[0-2])[/][0-9]{4})"
        );
        return regex.test(date);
    }

    /**
     * Chuyển đổi ngày tháng để đẩy lên api
     * Ngọc 18/7/2021
     */
    static convertDate(dateSrc) {
        let dateString = "";
        if (dateSrc) {
            let date = new Date(dateSrc),
                year = date.getFullYear().toString(),
                month = (date.getMonth() + 1).toString().padStart(2, "0"),
                day = date
                .getDate()
                .toString()
                .padStart(2, "0");

            dateString = `${year}-${month}-${day}`;
        } else {
            dateString = null;
        }
        return dateString;
    }

    /**
     * Lấy giá trị của một enum
     * Ngọc 18/7/2021
     * @param {*} data 
     * @param {*} enumName 
     * @returns 
     */
    static getValueEnum(data, enumName) {
        let enumeration = Enumeration[enumName],
            resource = Resource[enumName];

        for (var propName in enumeration) {
            if (enumeration[propName] == data) {
                data = resource[propName];
            }
        }

        return data;
    }

}