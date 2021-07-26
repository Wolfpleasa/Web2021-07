// Trang Khách Hàng
class CustomerPage extends BaseGrid {
    constructor(gridId, formId, warningFormId) {
        super(gridId, formId, warningFormId);

        this.config();
    }

    config() {
        let me = this,
            param = {
                entityName: "khách hàng",
            }
        Object.assign(me, param);
    }
}
$(document).ready(function() {
    let customerPage = new CustomerPage("#gridEntity", "#popup", "#warning-popup");
});